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
    /// API controller for person in workout
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonInWorkoutController : ControllerBase
    {
     
        private readonly IAppBLL _bll;
        private readonly IMapper _mapper;
        
        /// <summary>
        /// Constructor of the controller
        /// </summary>
        /// <param name="bll">App BLL</param>
        /// <param name="mapper">Mapper between BLL dto and API dto </param>
        public PersonInWorkoutController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = mapper;
        }

        // GET: api/PersonInWorkout
        /// <summary>
        /// Get all person's workouts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PersonInWorkout>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PersonInWorkout>>> GetPersonInWorkouts()
        {
            
            return (await _bll.PersonInWorkout.GetAllAsync(User.GetUserId()))
                .Select(x => _mapper.Map<PersonInWorkout>(x))
                .ToList();
        }

        // GET: api/PersonInWorkout/5
        /// <summary>
        /// Get all users in workout 
        /// </summary>
        /// <param name="id">Id of workout</param>
        /// <returns>List of persons in this workout</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PersonInWorkout>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PersonInWorkout>>> GetPersonInWorkout(Guid id)
        {
            
            var personInWorkout = (await _bll.PersonInWorkout.GetAllPersonInWorkoutByWorkoutId(id)).ToList();

            if (!personInWorkout.Any())
            {
                return NotFound();
            }

            return personInWorkout.Select(x => _mapper.Map<PersonInWorkout>(x)).ToList();
        }
        /// <summary>
        /// Get person in workout by id
        /// </summary>
        /// <param name="id">Person in workout by id</param>
        /// <returns>Person in workout dto</returns>
        [HttpGet("userInfo/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PersonInWorkout), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PersonInWorkout>> GetPersonInWorkoutInfo(Guid id)
        {

            var personInWorkout = await _bll.PersonInWorkout.FirstOrDefaultAsync(id);

            if (personInWorkout == null)
            {
                return NotFound();
            }

            return _mapper.Map<PersonInWorkout>(personInWorkout);
        }

        // PUT: api/PersonInWorkout/5
        /// <summary>
        /// Update person in workout information
        /// </summary>
        /// <param name="id">If of person in workout</param>
        /// <param name="personInWorkout">Person in workout with updated info</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutPersonInWorkout(Guid id, PersonInWorkout personInWorkout)
        {
            //Check for user role
            //Only for updating user comment on personInWorkout
            
            var personInWorkoutFromBll = await _bll.PersonInWorkout.FirstOrDefaultAsync(id);
            
            if (personInWorkoutFromBll == null) { return BadRequest(); }
            
            
            personInWorkoutFromBll.Comment = personInWorkout.Comment;
            
            _bll.PersonInWorkout.Update(personInWorkoutFromBll);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/PersonInWorkout
        /// <summary>
        /// Add person to workout
        /// </summary>
        /// <param name="personInWorkout">Person in workout dto</param>
        /// <returns>Person in workout dto with id</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PersonInWorkout), StatusCodes.Status200OK)]
        public async Task<ActionResult<PersonInWorkout>> PostPersonInWorkout(PersonInWorkout personInWorkout)
        {
            var addedPiW = _bll.PersonInWorkout.Add(_mapper.Map<App.BLL.DTO.PersonInWorkout>(personInWorkout));
            await _bll.SaveChangesAsync();
            var mappedPiW = _mapper.Map<PersonInWorkout>(addedPiW);
            return CreatedAtAction("GetPersonInWorkout", new { id = mappedPiW.Id }, mappedPiW);
        }

        // DELETE: api/PersonInWorkout/5
        /// <summary>
        /// Delete person from workout
        /// </summary>
        /// <param name="id">Id of person in workout to be deleted</param>
        /// <returns>No Content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePersonInWorkout(Guid id)
        {
            //Only Coach can access this 
            //Because from UI coach has access only workouts that he have created, therefore only he can delete 
            //players from workouts. Maybe extra check?
            
            
            var personInWorkout = await _bll.PersonInWorkout.FirstOrDefaultAsync(id);
            if (personInWorkout == null)
            {
                return NotFound();
            }

            await _bll.PersonInWorkout.RemoveAsync(personInWorkout.Id);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
