#nullable disable

using App.BLL.Contracts;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using Base.Extension;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using AppUser = PublicAPI.DTO.v1.Identity.AppUser;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// API controller for getting users
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        
        private readonly IMapper _mapper;
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor of the controller
        /// </summary>
        /// <param name="bll">App BLL</param>
        /// <param name="mapper">Mapper between BLL dto and API dto </param>
        public UserController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = mapper;
            
        }

        /// <summary>
        /// Get all club members by user id 
        /// </summary>
        /// <returns>List of club members</returns>
        [HttpGet]
        [Route("getallclubsmembers")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<AppUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicAPI.DTO.v1.Identity.AppUser>>> GetAllClubsMembers()
        {
            if (User.IsInRole("Coach"))
            {
                var userId = User.GetUserId();
              
                return (await _bll.Users.GetAllClubMembers(userId))
                    .Select(x => _mapper.Map<PublicAPI.DTO.v1.Identity.AppUser>(x))
                    .ToList();
            }
            
            return Unauthorized();
        }
    }
}