#nullable disable
using App.BLL.Contracts;
using App.BLL.DTO;
using AutoMapper;
using Base.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Club = PublicAPI.DTO.v1.Club;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// API controller for club.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClubController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor of the controller
        /// </summary>
        /// <param name="bll">App BLL</param>
        /// <param name="mapper">Mapper between BLL dto and API dto </param>
        public ClubController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = mapper;
        }

        // GET: api/Club
        /// <summary>
        /// Get all user owned clubs
        /// </summary>
        /// <returns>User owned clubs</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Club>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        
        public async Task<ActionResult<IEnumerable<Club>>> GetClubs()
        {
            if (User.IsInRole("Coach"))
            {
                return (await _bll.Club.GetAllAsync(User.GetUserId())).Select(x =>
                    _mapper.Map<Club>(x)!).ToList();
            }
            return Unauthorized();
        }
        
        // GET: api/Club
        /// <summary>
        /// Get all user opponents clubs
        /// </summary>
        /// <returns>User opponents clubs</returns>
        [HttpGet]
        [Route("opponentClubs")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Club>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        
        public async Task<ActionResult<IEnumerable<Club>>> GetUserOpponentClubs()
        {
            if (User.IsInRole("Coach"))
            {
                return (await _bll.Club.GetUserOpponentClubs(User.GetUserId())).Select(x =>
                    _mapper.Map<Club>(x)!).ToList();
            }
            return Unauthorized();
        }
        
        // GET: api/Club
        /// <summary>
        /// Get all user opponents clubs
        /// </summary>
        /// <returns>User opponents clubs</returns>
        [HttpGet]
        [Route("ownClubs")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Club>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        
        public async Task<ActionResult<IEnumerable<Club>>> GetUserClubs()
        {
            if (User.IsInRole("Coach"))
            {
                return (await _bll.Club.GetUserOwnedClubs(User.GetUserId())).Select(x =>
                    _mapper.Map<Club>(x)!).ToList();
            }
            return Unauthorized();
        }

        // GET: api/Club/5
        /// <summary>
        /// Get club by id
        /// </summary>
        /// <param name="id">Get club by id.</param>
        /// <returns>Club</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Club), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Club>> GetClub(Guid id)
        {
            var club = _mapper.Map<Club>(await _bll.Club.FirstOrDefaultAsync(id));

            if (club == null)
            {
                return NotFound();
            }

            return club;
        }


        // PUT: api/Club/5
        /// <summary>
        /// Update club info.
        /// </summary>
        /// <param name="id">Club id to be updated</param>
        /// <param name="club">Club dto with new information</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutClub(Guid id, Club club)
        {
            if (id != club.Id)
            {
                return BadRequest();
            }

            var mappedClub = _mapper.Map<App.BLL.DTO.Club>(club);
            mappedClub.AppUserId = User.GetUserId();
            _bll.Club.Update(mappedClub);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Club
        /// <summary>
        /// Create a new club
        /// </summary>
        /// <param name="club">Club dto</param>
        /// <returns>Created club with id</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Club), StatusCodes.Status200OK)]
        public async Task<ActionResult<Club>> PostClub(Club club)
        {
            var bllClub = _mapper.Map<App.BLL.DTO.Club>(club);
            bllClub.AppUserId = User.GetUserId();

            var addedClub = _mapper.Map<Club>(_bll.Club.Add(bllClub));
            /*if (addedClub.OwnClub)
            {
                _bll.PersonInClub.Add(new PersonInClub()
                {
                    AppUserId = User.GetUserId(),
                    ClubId = addedClub.Id
                });
            }*/
            await _bll.SaveChangesAsync();
            Console.WriteLine(addedClub.Id);

            return CreatedAtAction("GetClub", new { id = addedClub.Id }, addedClub);
        }

        // DELETE: api/Club/5
        /// <summary>
        /// Delete club with id
        /// </summary>
        /// <param name="id">Club id to be deleted</param>
        /// <returns>No Content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteClub(Guid id)
        {
            var club = await _bll.Club.FirstOrDefaultAsync(id);
            if (club == null)
            {
                return NotFound();
            }

            await _bll.Club.RemoveAsync(club.Id);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}