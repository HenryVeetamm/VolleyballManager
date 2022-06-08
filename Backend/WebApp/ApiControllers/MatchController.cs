#nullable disable
using App.BLL.Contracts;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Base.Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PublicAPI.DTO.v1;
using Match = PublicAPI.DTO.v1.Match;
using PersonInMatch = App.BLL.DTO.PersonInMatch;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// API controller for match.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MatchController : ControllerBase
    {
     
        private readonly IAppBLL _bll;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor of the controller
        /// </summary>
        /// <param name="bll">App BLL</param>
        /// <param name="mapper">Mapper between BLL dto and API dto </param>
        public MatchController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = mapper;
        }

        // GET: api/Match
        /// <summary>
        /// Get all user created matches.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Match>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatches()
        {
            var userId = User.GetUserId();
            return (await _bll.Match.GetAllMatchesAsync(userId)).Select(x => _mapper.Map<Match>(x)).ToList();
        }

        // GET: api/Match/5
        /// <summary>
        /// Get a match by id
        /// </summary>
        /// <param name="id">Match id </param>
        /// <returns>Match dto</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Match), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Match>> GetMatch(Guid id)
        {
            var match = await _bll.Match.FirstOrDefaultAsync(id, User.GetUserId());

            if (match == null)
            {
                return NotFound();
            }

            return _mapper.Map<Match>(match);
        }

        // PUT: api/Match/5
        /// <summary>
        /// Update match information
        /// </summary>
        /// <param name="id">Match id to be updated</param>
        /// <param name="match">Match dto with updated information.</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutMatch(Guid id, MatchUpdate match)
        {
            Console.WriteLine(match.Id);
            var matchBll = await _bll.Match.FirstOrDefaultAsync(id, User.GetUserId());
            if (matchBll == null)
            {
                return BadRequest();
            }

            var mappedMatch = _mapper.Map<App.BLL.DTO.Match>(match);
            mappedMatch.AppUserId = matchBll.AppUserId;

            _bll.Match.Update(mappedMatch);
            await _bll.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Match
        /// <summary>
        /// Method for creating a match
        /// </summary>
        /// <param name="match">Match to be created</param>
        /// <returns>Created match with id</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Match), StatusCodes.Status200OK)]
        public async Task<ActionResult<Match>> PostMatch(Match match)
        {
            var mappedBllMatch = _mapper.Map<App.BLL.DTO.Match>(match);
            mappedBllMatch.AppUserId = User.GetUserId();
            var addedMatch =_bll.Match.Add(mappedBllMatch);

            _bll.PersonInMatch.Add(new PersonInMatch()
            {
                AppUserId = User.GetUserId(),
                MatchId = addedMatch.Id
            });
            
            await _bll.SaveChangesAsync();
            var mappedMatch = _mapper.Map<Match>(addedMatch);

            return CreatedAtAction("GetMatch", new { id = mappedMatch.Id }, mappedMatch);
        }

        // DELETE: api/Match/5
        /// <summary>
        /// Delete match by given id 
        /// </summary>
        /// <param name="id">Match id to be deleted</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteMatch(Guid id)
        {
            var match = await _bll.Match.FirstOrDefaultAsync(id);
            if (match == null)
            {
                return NotFound();
            }

            await _bll.Match.RemoveAsync(match.Id);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
