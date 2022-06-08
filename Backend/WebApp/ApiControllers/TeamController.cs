#nullable disable

using App.BLL.Contracts;
using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Base.Extension;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PublicAPI.DTO.v1;
using Club = App.DAL.DTO.Club;
using Team = PublicAPI.DTO.v1.Team;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// API controller for team
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TeamController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly IMapper _mapper;
        
        /// <summary>
        /// Constructor of the controller
        /// </summary>
        /// <param name="bll">App BLL</param>
        /// <param name="mapper">Mapper between BLL dto and API dto </param>
        public TeamController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = mapper;
        }

        /*// GET: api/Team
        /// <summary>
        /// Get all teams
        /// </summary>
        /// <returns>List of teams</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Team>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            return (await _bll.Team.GetAllAsync())
                .Select(x => _mapper.Map<Team>(x))
                .ToList();
        }*/
        
        /// <summary>
        /// Get teams which club owner is user
        /// </summary>
        /// <returns>List of teams</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Team>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            if (User.IsInRole("Coach"))
            {
                var userId = User.GetUserId();
                var userClubTeams = await _bll.Team.GetAllAsync(userId);
                return userClubTeams.Select(x => _mapper.Map<Team>(x)).ToList();
            }

            return BadRequest();
        }
        
        /// <summary>
        /// Get teams which club owner is user
        /// </summary>
        /// <returns>List of teams</returns>
        [HttpGet]
        [Route("ownTeams")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Team>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Team>>> GetOwnTeams()
        {
            if (User.IsInRole("Coach"))
            {
                var userId = User.GetUserId();
                var userClubTeams = await _bll.Team.GetAllPersonTeamsByUserId(userId);
                return userClubTeams.Select(x => _mapper.Map<Team>(x)).ToList();
            }

            return BadRequest();
        }
        /// <summary>
        /// Get opponent teams that user has created
        /// </summary>
        /// <returns>List of teams</returns>
        [HttpGet]
        [Route("opponentTeams")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Team>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Team>>> GetOpponentTeams()
        {
            if (User.IsInRole("Coach"))
            {
                var userId = User.GetUserId();
                var userClubTeams = await _bll.Team.GetAllOpponentTeams(userId);
                return userClubTeams.Select(x => _mapper.Map<Team>(x)).ToList();
            }

            return BadRequest();
        }

        // GET: api/Team/5
        /// <summary>
        /// Get team by id 
        /// </summary>
        /// <param name="id">Id of team</param>
        /// <returns>Team dto</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Team), StatusCodes.Status200OK)]
        public async Task<ActionResult<Team>> GetTeam(Guid id)
        {
            var team = await _bll.Team.FirstOrDefaultAsync(id);

            if (team == null)
            {
                return NotFound();
            }

            return _mapper.Map<Team>(team);
        }

        // PUT: api/Team/5
        /// <summary>
        /// Update team
        /// </summary>
        /// <param name="id">Id of team to be updated</param>
        /// <param name="team">Team dto with updated information</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutTeam(Guid id, TeamUpdate team)
        {
            if (id != team.Id)
            {
                return BadRequest();
            }
            
            Console.WriteLine(team.ClubId);
            

            var mappedTeam = _mapper.Map<App.BLL.DTO.Team>(team);
            mappedTeam.AppUserId = User.GetUserId();
            /*mappedTeam.Club!.AppUserId = User.GetUserId();*/
            
            _bll.Team.Update(mappedTeam);
            await _bll.SaveChangesAsync();
            
            return NoContent();
        }

        // POST: api/Team
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Add team
        /// </summary>
        /// <param name="team">Team dto to be added</param>
        /// <returns>Team dto with id</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Team), StatusCodes.Status200OK)]
        public async Task<ActionResult<Team>> PostTeam(Team team)
        {   
            
            var mappedBllTeam = _mapper.Map<App.BLL.DTO.Team>(team);
            
            mappedBllTeam.AppUserId = User.GetUserId();
            var addedTeam = _bll.Team.Add(mappedBllTeam);
            
            await _bll.SaveChangesAsync();
            var mappedTeam = _mapper.Map<Team>(addedTeam);
            return CreatedAtAction("GetTeam", new { id = mappedTeam.Id }, mappedTeam);
        }

        // DELETE: api/Team/5
        /// <summary>
        /// Remove team
        /// </summary>
        /// <param name="id">Team id to be removed</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTeam(Guid id)
        {
            var team = await _bll.Team.FirstOrDefaultAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            await _bll.Team.RemoveAsync(team.Id);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
