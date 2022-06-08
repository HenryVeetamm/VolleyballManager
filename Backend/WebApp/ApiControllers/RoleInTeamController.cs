#nullable disable

using App.BLL.Contracts;

using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using Base.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PublicAPI.DTO.v1;


namespace WebApp.ApiControllers
{
    /// <summary>
    /// API controller for roles in team
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoleInTeamController : ControllerBase
    {
        
        private readonly IMapper _mapper;
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor of the controller
        /// </summary>
        /// <param name="bll">App BLL</param>
        /// <param name="mapper">Mapper between BLL dto and API dto </param>
        public RoleInTeamController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = mapper;
       
        }

        // GET: api/RoleInTeam
        /// <summary>
        /// Get all possible roles in team
        /// </summary>
        /// <returns>List of roles</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<RolesInTeam>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RolesInTeam>>> GetRolesInTeams()
        {
            return (await _bll.RolesInTeam.GetAllAsync())
                .Select(x => _mapper.Map<RolesInTeam>(x))
                .ToList();
        }

        // GET: api/RoleInTeam/5
        /// <summary>
        /// Get a role by id
        /// </summary>
        /// <param name="id">Id of role </param>
        /// <returns>Role dto</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RolesInTeam), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RolesInTeam>> GetRolesInTeam(Guid id)
        {
            var rolesInTeam = await _bll.RolesInTeam.FirstOrDefaultAsync(id);

            if (rolesInTeam == null)
            {
                return NotFound();
            }

            return _mapper.Map<RolesInTeam>(rolesInTeam);
        }

        // PUT: api/RoleInTeam/5
        /// <summary>
        /// Update role by id
        /// </summary>
        /// <param name="id">Id of role</param>
        /// <param name="rolesInTeam">Role in team dto with updated information</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutRolesInTeam(Guid id, RolesInTeam rolesInTeam)
        {
            var roleInTeam = await _bll.RolesInTeam.FirstOrDefaultAsync(id);
            if (roleInTeam == null) return BadRequest();
            
            
            if (id != roleInTeam.Id)
            {
                return BadRequest();
            }
            
            roleInTeam.RoleDescription.SetTranslation(rolesInTeam.RoleDescription);

            _bll.RolesInTeam.Update(roleInTeam);
            await _bll.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/RoleInTeam
        /// <summary>
        /// Add new role in team
        /// </summary>
        /// <param name="rolesInTeamDto">Role in team dto</param>
        /// <returns>Role in team dto with id</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RolesInTeam), StatusCodes.Status200OK)]
        public async Task<ActionResult<RolesInTeam>> PostRolesInTeam(RolesInTeam rolesInTeamDto)
        {
            var rolesInTeam = new App.BLL.DTO.RolesInTeam()
            {
                RoleDescription = new LangStr(rolesInTeamDto.RoleDescription)
            };
            var addedRiT = _bll.RolesInTeam.Add(rolesInTeam);
            await _bll.SaveChangesAsync();
            var mappedRit = _mapper.Map<RolesInTeam>(addedRiT);
            return CreatedAtAction("GetRolesInTeam", new { id = mappedRit.Id }, mappedRit);
        }

        // DELETE: api/RoleInTeam/5
        /// <summary>
        /// Remove role in team
        /// </summary>
        /// <param name="id">Id of role in team to be removed</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteRolesInTeam(Guid id)
        {
            var rolesInTeam = await _bll.RolesInTeam.FirstOrDefaultAsync(id);
            if (rolesInTeam == null)
            {
                return NotFound();
            }

            await _bll.RolesInTeam.RemoveAsync(rolesInTeam.Id);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
