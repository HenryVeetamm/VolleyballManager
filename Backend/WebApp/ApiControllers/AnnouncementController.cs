#nullable disable

using App.BLL.Contracts;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Base.Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PublicAPI.DTO.v1;
using PublicAPI.DTO.v1.Mappers;

namespace WebApp.ApiControllers
{   
    /// <summary>
    /// API controller for announcement.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly AnnouncementMapper _mapper;

        /// <summary>
        /// Constructor of the controller
        /// </summary>
        /// <param name="bll">App BLL</param>
        /// <param name="mapper">Mapper between BLL dto and API dto </param>
        public AnnouncementController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new AnnouncementMapper(mapper);
        }


        // GET: api/Announcement
        /// <summary>
        /// Get all announcements considering if user is a player or a coach.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Announcement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Announcement>>> GetAnnouncements()
        {
            if (User.IsInRole("Player"))
            {
                return (await _bll.Announcement.GetAllPlayerAnnouncements(User.GetUserId()))
                    .Select(x => _mapper.Map(x))
                    .ToList();
            }

            if (User.IsInRole("Coach"))
            {
                return (await _bll.Announcement.GetAllAsync(User.GetUserId()))
                    .Select(x => _mapper.Map(x)).ToList();
            }

            return BadRequest();
        }

        // GET: api/Announcement/5
        /// <summary>
        /// Get the announcement by id.
        /// </summary>
        /// <param name="id">Announcement id</param>
        /// <returns>Announcement</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Announcement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Announcement>> GetAnnouncement(Guid id)
        {
            var announcement = await _bll.Announcement.FirstOrDefaultAsync(id);

            if (announcement == null)
            {
                return NotFound();
            }

            return _mapper.Map(announcement);
        }

        // PUT: api/Announcement/5
        /// <summary>
        /// Update an announcement.
        /// </summary>
        /// <param name="id">Id of announcement to be updated</param>
        /// <param name="announcement">Announcement dto with updated information</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutAnnouncement(Guid id, AnnouncementPost announcement)
        {
            if (id != announcement.Id)
            {
                return BadRequest();
            }

            var mapped = _mapper.MapPost(announcement);
            mapped!.AppUserId = User.GetUserId();
            
            _bll.Announcement.Update(mapped);
            await _bll.SaveChangesAsync();

            return NoContent();
            
        }

        // POST: api/Announcement
        /// <summary>
        ///  Create an announcement
        /// </summary>
        /// <param name="announcement">Announcement to be created</param>
        /// <returns>Created announcement with id</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Announcement), StatusCodes.Status200OK)]
        public async Task<ActionResult<Announcement>> PostAnnouncement(Announcement announcement)
        {
            announcement.AppUserId = User.GetUserId();
            _bll.Announcement.Add(_mapper.Map(announcement)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetAnnouncement", new { id = announcement.Id }, announcement);
        }

        // DELETE: api/Announcement/5
        /// <summary>
        /// Delete an announcement by id
        /// </summary>
        /// <param name="id">Announcement id to be deleted</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAnnouncement(Guid id)
        {
            var announcement = await _bll.Announcement.FirstOrDefaultAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            await _bll.Announcement.RemoveAsync(id);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
