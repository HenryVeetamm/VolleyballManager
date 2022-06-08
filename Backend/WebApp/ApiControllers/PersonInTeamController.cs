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
    /// API controller for person in team
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonInTeamController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor of the controller
        /// </summary>
        /// <param name="bll">App BLL</param>
        /// <param name="mapper">Mapper between BLL dto and API dto </param>
        public PersonInTeamController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = mapper;
        }

        // GET: api/PersonInTeam
        /// <summary>
        /// Get all teams where person (user) belongs
        /// </summary>
        /// <returns>List of teams(PersonInTeam dto) where user belongs</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PersonInTeam>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PersonInTeam>>> GetPersonInTeams()
        {
            return (await _bll.PersonInTeam.GetAllAsync(User.GetUserId()))
                .Select(x => _mapper.Map<PersonInTeam>(x))
                .ToList();
        }

        // GET: api/PersonInTeam/5
        /// <summary>
        /// Shows every person in a specified team
        /// </summary>
        /// <param name="id">Id of team</param>
        /// <returns>Returns a list of players in team with their roles</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PersonInTeam), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PersonInTeam>>> GetPersonInTeam(Guid id)
        {
            //Coach sees everyone in this team!
            //Needs better DTO

            var personInTeams = (await _bll.PersonInTeam.GetAllPersonInTeamByTeamId(id)).ToList();


            return personInTeams.Select(x => _mapper.Map<PersonInTeam>(x)).ToList();
        }

        // PUT: api/PersonInTeam/5
        /// <summary>
        /// Update person in team
        /// </summary>
        /// <param name="id">Id of person in team</param>
        /// <param name="personInTeam">Person in team dto with updated information </param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutPersonInTeam(Guid id, PersonInTeam personInTeam)
        {
            //ToDo : Additional check if updating personInTeam is allowed
            //Todo: User should be coach and belong to the same team
            
            var personInTeamFromUow = await _bll.PersonInTeam.FirstOrDefaultAsync(id);
            if (personInTeamFromUow == null)
            {
                return BadRequest();
            }

            if (id != personInTeam.Id)
            {
                return BadRequest();
            }

            _bll.PersonInTeam.Update(_mapper.Map<App.BLL.DTO.PersonInTeam>(personInTeam));
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/PersonInTeam
        /// <summary>
        /// Add person to team
        /// </summary>
        /// <param name="personInTeam">Person in team dto with information</param>
        /// <returns>Person in team dto with id</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PersonInTeam), StatusCodes.Status200OK)]
        public async Task<ActionResult<PersonInTeam>> PostPersonInTeam(PersonInTeam personInTeam)
        {
            var addedPiT = _bll.PersonInTeam.Add(_mapper.Map<App.BLL.DTO.PersonInTeam>(personInTeam));
            await _bll.SaveChangesAsync();
            var mappedPiT = _mapper.Map<PersonInTeam>(addedPiT);
            
            return CreatedAtAction("GetPersonInTeam", new { id = mappedPiT.Id }, mappedPiT);
        }

        // DELETE: api/PersonInTeam/5
        /// <summary>
        /// Remove person from team
        /// </summary>
        /// <param name="id">Id of person in team</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePersonInTeam(Guid id)
        {
            var personInTeam = await _bll.PersonInTeam.FirstOrDefaultAsync(id);
            if (personInTeam == null)
            {
                return NotFound();
            }

            await _bll.PersonInTeam.RemoveAsync(personInTeam.Id);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}