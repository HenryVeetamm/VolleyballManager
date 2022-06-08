#nullable disable

using App.BLL.Contracts;
using App.DAL.Contracts;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Base.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PublicAPI.DTO.v1;
using PublicAPI.DTO.v1.Mappers;
using WebApp.DTO;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// API controller for workout type
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    /*[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]*/
    public class WorkoutTypeController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly WorkoutTypeMapper _mapper;


        /// <summary>
        /// Constructor of the controller
        /// </summary>
        /// <param name="bll">App BLL</param>
        /// <param name="mapper">Mapper between BLL dto and API dto </param>
        public WorkoutTypeController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new WorkoutTypeMapper(mapper);
        }

        // GET: api/WorkoutType
        
        /// <summary>
        /// Get all workout types
        /// </summary>
        /// <returns>List of workout types</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<WorkoutType>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WorkoutType>>> GetWorkoutTypes()
        {
            var res = (await _bll.WorkoutType.GetAllAsync())
                .Select(x => _mapper.Map(x)).ToList();

            return res;
        }

        // GET: api/WorkoutType/5
        /// <summary>
        /// Get workout type by id
        /// </summary>
        /// <param name="id">Workout type id </param>
        /// <returns>Workout type dto</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(WorkoutType), StatusCodes.Status200OK)]
        public async Task<ActionResult<WorkoutType>> GetWorkoutType(Guid id)
        {
            var workoutType = await _bll.WorkoutType.FirstOrDefaultAsync(id);

            if (workoutType == null)
            {
                return NotFound();
            }

            return _mapper.Map(workoutType);
        }

        // PUT: api/WorkoutType/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update workout type
        /// </summary>
        /// <param name="id">Workout type id</param>
        /// <param name="workoutTypeDto">Workout type dto with updated information</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutWorkoutType(Guid id, WorkoutType workoutTypeDto)
        {
            var workoutType = await _bll.WorkoutType.FirstOrDefaultAsync(id);
            if (workoutType == null) return BadRequest();


            if (id != workoutType.Id)
            {
                return BadRequest();
            }

            workoutType.Description.SetTranslation(workoutTypeDto.Description);

            _bll.WorkoutType.Update(workoutType);
            await _bll.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/WorkoutType
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Add a new workout type
        /// </summary>
        /// <param name="workoutType">Workout type dto with information</param>
        /// <returns>Workout type dto with id</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(WorkoutType), StatusCodes.Status200OK)]
        public async Task<ActionResult<WorkoutType>> PostWorkoutType(WorkoutType workoutType)
        {
            var bllWorkoutType = _mapper.Map(workoutType);
            var afterAddWorkoutType = _bll.WorkoutType.Add(bllWorkoutType!);
            await _bll.SaveChangesAsync();
            

            return CreatedAtAction("GetWorkoutType", new { id = afterAddWorkoutType.Id },
                _mapper.Map(afterAddWorkoutType));
        }

        // DELETE: api/WorkoutType/5
        /// <summary>
        /// Delete workout type
        /// </summary>
        /// <param name="id">Workout type id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteWorkoutType(Guid id)
        {
            var workoutType = await _bll.WorkoutType.FirstOrDefaultAsync(id);
            if (workoutType == null)
            {
                return NotFound();
            }

            await _bll.WorkoutType.RemoveAsync(workoutType.Id);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}