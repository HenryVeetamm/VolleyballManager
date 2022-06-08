#nullable disable

using App.BLL.Contracts;
using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc;


using AutoMapper;
using Base.Extension;
using DAL.App.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Workout = PublicAPI.DTO.v1.Workout;


namespace WebApp.ApiControllers
{
    /// <summary>
    /// API controller for workout
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WorkoutController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor of the controller
        /// </summary>
        /// <param name="bll">App BLL</param>
        /// <param name="mapper">Mapper between BLL dto and API dto </param>
        public WorkoutController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = mapper;
        }

        // GET: api/Workout
        /// <summary>
        /// Get all workout
        /// </summary>
        /// <returns>List of workouts</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Workout>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Workout>>> GetWorkouts()
        {
            
            return (await _bll.Workout.GetAllAsync(User.GetUserId()))
                .Select(x => _mapper.Map<Workout>(x))
                .ToList();
        }

        // GET: api/Workout/5
        /// <summary>
        /// Get workout by id
        /// </summary>
        /// <param name="id">Id of workout</param>
        /// <returns>Workout dto with workout type</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Workout), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Workout>> GetWorkout(Guid id)
        {
            var workout = await _bll.Workout.FirstOrDefaultAsync(id);

            if (workout == null)
            {
                return NotFound();
            }

            return _mapper.Map<Workout>(workout);
        }

        // PUT: api/Workout/5
        /// <summary>
        /// Update workout information
        /// </summary>
        /// <param name="id">Id of workout</param>
        /// <param name="workout">Workout dto with updated information</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutWorkout(Guid id, Workout workout)
        {
            if (id != workout.Id)
            {
                return BadRequest();
            }

            var mappedWorkout = _mapper.Map<App.BLL.DTO.Workout>(workout);
            mappedWorkout.AppUserId = User.GetUserId();
            _bll.Workout.Update(mappedWorkout);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Workout
        /// <summary>
        /// Add new workout
        /// </summary>
        /// <param name="workout">Workout dto with information</param>
        /// <returns>Workout dto with id</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Workout), StatusCodes.Status200OK)]
        public async Task<ActionResult<Workout>> PostWorkout(Workout workout)
        {
            var bllWorkout = _mapper.Map<App.BLL.DTO.Workout>(workout);
            bllWorkout.AppUserId = User.GetUserId();
            
            var addedWorkout =_bll.Workout.Add(bllWorkout);
            _bll.PersonInWorkout.Add(new PersonInWorkout()
            {
                WorkOutId = addedWorkout.Id,
                AppUserId = User.GetUserId()
            });
            await _bll.SaveChangesAsync();
            var mappedWorkout = _mapper.Map<Workout>(addedWorkout);
            return CreatedAtAction("GetWorkout", new { id = mappedWorkout.Id }, mappedWorkout);
        }

        // DELETE: api/Workout/5
        /// <summary>
        /// Delete workout
        /// </summary>
        /// <param name="id">Workout id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteWorkout(Guid id)
        {
            var workout = await _bll.Workout.FirstOrDefaultAsync(id);
            if (workout == null)
            {
                return NotFound();
            }

            await _bll.Workout.RemoveAsync(workout.Id);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
