using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using App.Domain.Enums;
using App.Domain.Identity;
using AutoMapper;
using Base.Extension;
using DAL.App.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PublicAPI.DTO.v1.Account;


namespace WebApp.ApiControllers.Identity;

/// <summary>
/// Controller for account actions
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/identity/[controller]/[action]")]
[ApiController]

public class AccountController : ControllerBase
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly Random _rnd = new();
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;


    /// <summary>
    /// Constructor for controller.
    /// </summary>
    /// <param name="signInManager">Sign in manager</param>
    /// <param name="logger">Logger</param>
    /// <param name="userManager">User manager</param>
    /// <param name="configuration">Configuration</param>
    /// <param name="context">Database context</param>
    public AccountController(SignInManager<AppUser> signInManager,
        ILogger<AccountController> logger,
        UserManager<AppUser> userManager,
        IConfiguration configuration, AppDbContext context,
        IMapper mapper)
    {
        _signInManager = signInManager;
        _logger = logger;
        _userManager = userManager;
        _configuration = configuration;
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Log in an user
    /// </summary>
    /// <param name="loginDto">Login dto</param>
    /// <returns>JWT response with JWT token and refresh token</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(JwtResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JwtResponse>> LogIn([FromBody] Login loginDto)
    {
        var errorResponse = new RestApiErrorResponse(HttpStatusCode.BadRequest,
            Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            "User",
            new List<string>(){"User/Password problem"});
        
        
        //verify username
        var appUser = await _userManager.FindByEmailAsync(loginDto.Email);
        if (appUser == null)
        {
            _logger.LogWarning("WebApi login failed, email {} not found", loginDto.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound(errorResponse);
        }

        //verify username and password
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginDto.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("WebApi login failed, password problem for user {}", loginDto.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound(errorResponse);
        }

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get ClaimsPrincipal for user {}", loginDto.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound(errorResponse);
        }

        appUser.RefreshTokens = await _context.Entry(appUser).Collection(a => a.RefreshTokens!)
            .Query().Where(t => t.AppUserId == appUser.Id).ToListAsync();

        foreach (var userRefreshToken in appUser.RefreshTokens)
        {
            if (userRefreshToken.ExpirationDateTime < DateTime.UtcNow &&
                userRefreshToken.PreviousTokenExpirationDateTime < DateTime.UtcNow)
            {
                _context.RefreshTokens.Remove(userRefreshToken);
            }
        }

        var refreshToken = new RefreshToken
        {
            AppUserId = appUser.Id
        };
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        //generate jwt
        var jwt = IdentityExtension.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"],
            _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes")));

        var res = new JwtResponse()
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName
        };

        return Ok(res);
    }


    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="registrationDto">Registration information</param>
    /// <returns>JWT response with JWT token and refresh token</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(JwtResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JwtResponse>> Register([FromBody] Register registrationDto)
    {
        //verify user
        var appUser = await _userManager.FindByEmailAsync(registrationDto.Email);
        if (appUser != null)
        {
            _logger.LogWarning("User with email {} is already registered", registrationDto.Email);
            
            var errorResponse = new RestApiErrorResponse(
                HttpStatusCode.BadRequest,
                Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                "Email",
                new List<string>()
                {
                    $"Email already registered"
                }
            );
            return BadRequest(errorResponse);
        }

        //create user
        var refreshToken = new RefreshToken();
        appUser = new AppUser()
        {
            Email = registrationDto.Email,
            NationalCode = registrationDto.Nationalcode,
            FirstName = registrationDto.FirstName,
            LastName = registrationDto.LastName,
            Gender = registrationDto.Gender == 0 ? EGender.Male : EGender.Female,
            Birthday = registrationDto.Birthday,
            UserName = registrationDto.Email,
            RefreshTokens = new List<RefreshToken>()
            {
                refreshToken
            }
        };


        var result = await _userManager.CreateAsync(appUser, registrationDto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        //get full user
        appUser = await _userManager.FindByEmailAsync(appUser.Email);

        await _userManager.AddToRoleAsync(appUser, registrationDto.Role);
        if (appUser == null)
        {
            _logger.LogWarning("User with email {} is not found after registration", registrationDto.Email);

            var errorResponse = new RestApiErrorResponse(
                HttpStatusCode.BadRequest,
                Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                "User",
                new List<string>()
                {
                    $"User with email {registrationDto.Email} is not found after registration"
                }
            );

            return BadRequest(errorResponse);
        }

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get ClaimsPrincipal for user {}", registrationDto.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            
            var errorResponse = new RestApiErrorResponse(
                HttpStatusCode.NotFound,
                Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                "User",
                new List<string>() { "User/Password problem" });
            
            return NotFound(errorResponse);
        }

        //generate jwt
        var jwt = IdentityExtension.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"],
            _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes")));

        var res = new JwtResponse()
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName
        };

        return Ok(res);
    }

    /// <summary>
    /// Refresh tokens
    /// </summary>
    /// <param name="refreshTokenModel">JWT token and refresh token</param>
    /// <returns>JWT response with new JWT token and refresh token</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(JwtResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JwtResponse>> RefreshToken([FromBody] RefreshTokenModel refreshTokenModel)
    {
        //get user info from jwt
        JwtSecurityToken jwtToken;
        try
        {
            jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenModel.Jwt);
            if (jwtToken == null)
            {
                var errorResponse = new RestApiErrorResponse(
                    HttpStatusCode.BadRequest,
                    Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    "Token",
                    new List<string>() { "No JWT token" });
                return BadRequest(errorResponse);
            }
        }
        catch (Exception e)
        {
            var errorResponse = new RestApiErrorResponse(
                HttpStatusCode.BadRequest,
                Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                "Token",
                new List<string>() { $"Cant parse the token, {e.Message}" });
            
            return BadRequest(errorResponse);
        }

        //Validate jwt signature
        
        var validJwtToken = IdentityExtension.ValidateJwtSignature(refreshTokenModel.Jwt, 
            _configuration["JWT:Key"],
            _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"]);
        if (!validJwtToken)
        {
            var errorResponse = new RestApiErrorResponse(
                HttpStatusCode.BadRequest,
                Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                "Token",
                new List<string>() { $"JWT validation failed" });
            
            return BadRequest(errorResponse);
        }

        var userEmail = jwtToken.Claims.First(x => x.Type == ClaimTypes.Email)?.Value;

        if (userEmail == null)
        {
            var errorResponse = new RestApiErrorResponse(
                HttpStatusCode.BadRequest,
                Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                "Token",
                new List<string>() { $"Email not found in JWT" });
            return BadRequest(errorResponse);
        }

        //get user and tokens
        var appUser = await _userManager.FindByEmailAsync(userEmail);
        if (appUser == null)
        {
            var errorResponse = new RestApiErrorResponse(
                HttpStatusCode.NotFound,
                Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                "User",
                new List<string>() { $"User with email {userEmail} not found" });
            
            return NotFound(errorResponse);
        }

        //compare refresh tokens
        await _context.Entry(appUser).Collection(u => u.RefreshTokens!)
            .Query()
            .Where(x => (x.Token == refreshTokenModel.RefreshToken && x.ExpirationDateTime > DateTime.UtcNow)
                        || (x.PreviousToken == refreshTokenModel.RefreshToken &&
                        x.PreviousTokenExpirationDateTime > DateTime.UtcNow))
            .ToListAsync();
        
        
        if (appUser.RefreshTokens == null)
        {
            return Problem("RefreshTokens collection is null");
        }

        if (appUser.RefreshTokens.Count == 0)
        {
            return Problem("RefreshTokens collection is empty, no valid refresh tokens found");
        }

        if (appUser.RefreshTokens.Count != 1)
        {
            return Problem("More than one valid refresh token found");
        }

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get ClaimsPrincipal for user {}", userEmail);
            await Task.Delay(_rnd.Next(100, 1000));
            
            var errorResponse = new RestApiErrorResponse(
                HttpStatusCode.NotFound,
                Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                "User",
                new List<string>() { "User/Password problem" });
            
            return NotFound(errorResponse);
        }

        //generate jwt
        var jwt = IdentityExtension.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"],
            _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes")));

        var refreshToken = appUser.RefreshTokens.First();

        if (refreshToken.Token == refreshTokenModel.RefreshToken)
        {
            refreshToken.PreviousToken = refreshToken.Token;
            refreshToken.PreviousTokenExpirationDateTime = DateTime.UtcNow.AddMinutes(1);

            refreshToken.Token = Guid.NewGuid().ToString();
            refreshToken.ExpirationDateTime = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();
        }

        var res = new JwtResponse()
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName
        };
        
        return Ok(res);
    }
    
    // GET: api/User
    /// <summary>
    /// Get all players
    /// </summary>
    /// <returns>List of players</returns>
    [HttpGet]
    [Route("getplayers")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<PublicAPI.DTO.v1.Identity.AppUser>), StatusCodes.Status200OK)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IEnumerable<PublicAPI.DTO.v1.Identity.AppUser>> GetPlayers()
    {
        var userId = User.GetUserId();
        var playersOnly = await _userManager!.GetUsersInRoleAsync("Player");

        return playersOnly.Where(x => x.Id != userId).Select(x => _mapper.Map<PublicAPI.DTO.v1.Identity.AppUser>(x)).ToList();
    }
}