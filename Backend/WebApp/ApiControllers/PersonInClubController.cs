#nullable disable

using App.BLL.Contracts;

using Microsoft.AspNetCore.Mvc;


using AutoMapper;

using Base.Extension;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PublicAPI.DTO.v1;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// API controller for person in club
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonInClubController : ControllerBase
    {

        private readonly IAppBLL _bll;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor of the controller
        /// </summary>
        /// <param name="bll">App BLL</param>
        /// <param name="mapper">Mapper between BLL dto and API dto </param>
        public PersonInClubController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = mapper;
        }

        // GET: api/PersonInClub
        /// <summary>
        /// Get all club members of all clubs where user is in.
        /// </summary>
        /// <returns>All club members</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PersonInClub>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PersonInClub>>> GetPersonInClubs()
        {
            return (await _bll.PersonInClub.GetAllUserClubs(User.GetUserId()))
                .Select(x => _mapper.Map<PersonInClub>(x)).ToList();
        }

        // GET: api/PersonInClub/5
        /// <summary>
        /// Get person in club by id. 
        /// </summary>
        /// <param name="id">Person in club id</param>
        /// <returns>Person in club dto</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PersonInClub), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PersonInClub>> GetPersonInClub(Guid id)
        {
            var personInClub = await _bll.PersonInClub.FirstOrDefaultAsync(id, User.GetUserId());

            return personInClub == null ? NotFound() : _mapper.Map<PersonInClub>(personInClub);
        }

        /// <summary>
        /// Return all specific club members
        /// </summary>
        /// <param name="id">Id of club</param>
        /// <returns>List of person in clubs</returns>
        [HttpGet( "getclubmembers/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PersonInClub>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PersonInClub>>> GetClubMembers(Guid id)
        {
            return (await _bll.PersonInClub.GetAllMembersOfClub(id))
                .Select(x => _mapper.Map<PersonInClub>(x))
                .ToList();
        }

        /*// PUT: api/PersonInClub/5
        /// <summary>
        /// Update person in club. 
        /// </summary>
        /// <param name="id">Id of person in club</param>
        /// <param name="personInClub">Updating</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutPersonInClub(Guid id, PersonInClub personInClub)
        {
            var personInClubFromUow = await _bll.PersonInClub.FirstOrDefaultAsync(id, User.GetUserId());
            
            if (personInClubFromUow == null) { return BadRequest(); }

            if (id != personInClub.Id) { return BadRequest(); }

            _bll.PersonInClub.Update(_mapper.Map<App.BLL.DTO.PersonInClub>(personInClub));
            await _bll.SaveChangesAsync();
            return NoContent();
            
        }*/

        // POST: api/PersonInClub
        /// <summary>
        /// Add person to club
        /// </summary>
        /// <param name="personInClub">Person in club dto, person to be added to which club</param>
        /// <returns>Person in club dto</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PersonInClub), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonInClub>> PostPersonInClub(PersonInClub personInClub)
        {
            Console.WriteLine("ERRRORRRRRR");
            Console.WriteLine(personInClub.ClubId);
            Console.WriteLine(personInClub.AppUserId);
            
            var addedPiC= _bll.PersonInClub.Add(_mapper.Map<App.BLL.DTO.PersonInClub>(personInClub));
            await _bll.SaveChangesAsync();
            var mappedPiC = _mapper.Map<PersonInClub>(addedPiC);
            return CreatedAtAction("GetPersonInClub", new { id = mappedPiC.Id }, mappedPiC);
        }

        // DELETE: api/PersonInClub/5
        /// <summary>
        /// Delete person from club
        /// </summary>
        /// <param name="id">Person in club id to be deleted</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePersonInClub(Guid id)
        {
            var personInClub = await _bll.PersonInClub.FirstOrDefaultAsync(id);
            if (personInClub == null)
            {
                return NotFound();
            }

            await _bll.PersonInClub.RemoveAsync(personInClub.Id);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
