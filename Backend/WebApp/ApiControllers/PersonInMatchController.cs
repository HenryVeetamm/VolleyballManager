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
    /// API controller for person in match
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonInMatchController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor of the controller
        /// </summary>
        /// <param name="bll">App BLL</param>
        /// <param name="mapper">Mapper between BLL dto and API dto </param>
        public PersonInMatchController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = mapper;
        }

        // GET: api/PersonInMatch
        /// <summary>
        /// Get all user matches
        /// </summary>
        /// <returns>Returns a list of person matches, with statistics</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PersonInMatch>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PersonInMatch>>> GetPersonInMatches()
        {
           
            return (await _bll.PersonInMatch.GetAllDetailedPersonInMatch(User.GetUserId()))
                .Select(x => _mapper.Map<PersonInMatch>(x)).ToList();
        }

        // GET: api/PersonInMatch/5
        /// <summary>
        /// Get a specific match with user information
        /// </summary>
        /// <param name="id"> Person in match id</param>
        /// <returns>Person in match dto with user information</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PersonInMatch), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PersonInMatch>> GetPersonInMatch(Guid id)
        {
            var personInMatch = await _bll.PersonInMatch.FirstOrDefaultAsync(id);

            if (personInMatch == null)
            {
                return NotFound();
            }

            return _mapper.Map<PersonInMatch>(personInMatch);
        }
        // GET: api/PersonInMatch/5
        /// <summary>
        /// Get all users in specified match
        /// </summary>
        /// <param name="id">Match id </param>
        /// <returns>Person in match dtos with user information</returns>
        [HttpGet("matchmembers/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PersonInMatch>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PersonInMatch>>> GetMembersInMatch(Guid id)
        {
            var personInMatches = (await _bll.PersonInMatch.GetAllPersonInMatchByMatchId(id))
                .ToList();

            if (!personInMatches.Any())
            {
                return NotFound() ;
            }

            return personInMatches.Select(x => _mapper.Map<PersonInMatch>(x)).ToList();
        }

        // PUT: api/PersonInMatch/5
        /// <summary>
        /// Update person in match information
        /// </summary>
        /// <param name="id">Id of person in match to be updated</param>
        /// <param name="personInMatch">Person in match dto with updated information</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutPersonInMatch(Guid id, PersonInMatch personInMatch)
        {
            if (id != personInMatch.Id)
            {
                return BadRequest();
            }

            _bll.PersonInMatch.Update(_mapper.Map<App.BLL.DTO.PersonInMatch>(personInMatch));
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/PersonInMatch
        /// <summary>
        /// Add a person to a match. 
        /// </summary>
        /// <param name="personInMatch">Person in match dto with person statistics</param>
        /// <returns>Person in match with id</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PersonInMatch), StatusCodes.Status200OK)]
        public async Task<ActionResult<PersonInMatch>> PostPersonInMatch(PersonInMatch personInMatch)
        {
            var addedPiM = _bll.PersonInMatch.Add(_mapper.Map<App.BLL.DTO.PersonInMatch>(personInMatch));
            await _bll.SaveChangesAsync();
            var mappedPiM = _mapper.Map<PersonInMatch>(addedPiM);
            return CreatedAtAction("GetPersonInMatch", new { id = mappedPiM.Id }, mappedPiM);
        }

        // DELETE: api/PersonInMatch/5
        /// <summary>
        /// Remove person in match 
        /// </summary>
        /// <param name="id">Id of "person in match"</param>
        /// <returns>No Content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePersonInMatch(Guid id)
        {
            var personInMatch = await _bll.PersonInMatch.FirstOrDefaultAsync(id);
            if (personInMatch == null)
            {
                return NotFound();
            }

            await _bll.PersonInMatch.RemoveAsync(id);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
        
    }
}
