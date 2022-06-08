#nullable disable

using App.BLL.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Base.Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PublicAPI.DTO.v1;


namespace WebApp.ApiControllers
{
    /// <summary>
    /// API controller for saved comparison
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SavedComparisonController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor of the controller
        /// </summary>
        /// <param name="bll">App BLL</param>
        /// <param name="mapper">Mapper between BLL dto and API dto </param>
        public SavedComparisonController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = mapper;
        }

        // GET: api/SavedComparison
        /// <summary>
        /// Get a list of logged in user saved comparisons
        /// </summary>
        /// <returns>List of saved comparisons</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<SavedComparison>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SavedComparison>>> GetSavedComparisons()
        {
            return (await _bll.SavedComparison.GetPlayerComparisonsByUserId(User.GetUserId()))
                .Select(x => _mapper.Map<SavedComparison>(x))
                .ToList();
        }

        // GET: api/SavedComparison/5
        /// <summary>
        /// Get detailed comparison of comparison
        /// </summary>
        /// <param name="id">Id of saved comparison</param>
        /// <returns>Details of saved comparison</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<SavedComparisonDetailed>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<SavedComparisonDetailed>>> GetSavedComparison(Guid id)
        {
            var savedComparison =
                (await _bll.SavedComparison.GetDetailedSavedComparison(id, User.GetUserId())).ToList();

            if (!savedComparison.Any())
            {
                return NotFound();
            }

            return (savedComparison).Select(x => _mapper.Map<SavedComparisonDetailed>(x)).ToList();
        }

        /*// PUT: api/SavedComparison/5
        /// <summary>
        /// Update saved comparison
        /// </summary>
        /// <param name="id">Id of saved comparison</param>
        /// <param name="savedComparison">Saved comparison dto with updated info</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutSavedComparison(Guid id, SavedComparison savedComparison)
        {
            var savedComparisonBll = await _bll.RolesInTeam.FirstOrDefaultAsync(id);
            if (savedComparisonBll == null) return BadRequest();
            
            
            if (id != savedComparison.Id)
            {
                return BadRequest();
            }

            _bll.SavedComparison.Update(_mapper.Map<App.BLL.DTO.SavedComparison>(savedComparison));
            await _bll.SaveChangesAsync();
            return NoContent();
        }*/

        // POST: api/SavedComparison
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Add saved comparison
        /// </summary>
        /// <param name="savedComparisonDto">Saved comparison dto</param>
        /// <returns>Saved comparison with id</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SavedComparison), StatusCodes.Status200OK)]
        public async Task<ActionResult<SavedComparison>> PostSavedComparison(SavedComparison savedComparisonDto)
        {
            var savedComparison = new SavedComparison()
            {
                ComparerId = User.GetUserId(),
                ComparableId = savedComparisonDto.ComparableId
            };


            var addedSc = _bll.SavedComparison.Add(_mapper.Map<App.BLL.DTO.SavedComparison>(savedComparison));
            await _bll.SaveChangesAsync();
            var mappedSc = _mapper.Map<SavedComparison>(addedSc);

            return CreatedAtAction("GetSavedComparison", new { id = mappedSc.Id }, mappedSc);
        }

        // DELETE: api/SavedComparison/5
        /// <summary>
        /// Delete saved comparison 
        /// </summary>
        /// <param name="id">Id of saved comparison</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteSavedComparison(Guid id)
        {
            
            var savedComparison = await _bll.SavedComparison.FirstOrDefaultAsync(id);
            if (savedComparison == null)
            {
                return NotFound();
            }
            
            await _bll.SavedComparison.RemoveAsync(id);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}