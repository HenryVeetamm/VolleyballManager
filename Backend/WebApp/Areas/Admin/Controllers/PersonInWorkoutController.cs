#nullable disable
using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.DTO.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Base.Extension;



namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PersonInWorkoutController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public PersonInWorkoutController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Admin/PersonInWorkout
        public async Task<IActionResult> Index()
        {
            var userId = User.GetUserId();
            
            return View((await _uow.PersonInWorkout.GetAllAsync(userId)).ToList());
        }

        // GET: Admin/PersonInWorkout/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            /*if (id == null)
            {
                return NotFound();
            }*/

            var personInWorkout = await _uow.PersonInWorkout.GetAllPersonInWorkoutByWorkoutId(id.Value);
            
            return !personInWorkout.Any() ? NotFound() : View(personInWorkout);
        }

        // GET: Admin/PersonInWorkout/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AppUserId"] = new SelectList(await GetClubMembers(), "Id", "FirstName");
            ViewData["WorkOutId"] = new SelectList(await _uow.Workout.GetAllAsync(), "Id", "Description");
            return View();
        }

        // POST: Admin/PersonInWorkout/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("WorkOutId, AppUserId, Comment,Id")] PersonInWorkout personInWorkout)
        {
            if (ModelState.IsValid)
            {
                _uow.PersonInWorkout.Add(personInWorkout);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            

            ViewData["AppUserId"] = new SelectList(await GetClubMembers(), "Id", "FirstName", personInWorkout.AppUserId);
            ViewData["WorkOutId"] = new SelectList(await _uow.Workout.GetAllAsync(), "Id", "Id", personInWorkout.WorkOutId);
            return View(personInWorkout);
        }

        // GET: Admin/PersonInWorkout/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInWorkout = await _uow.PersonInWorkout.FirstOrDefaultAsync(id.Value, User.GetUserId());
            if (personInWorkout == null)
            {
                return NotFound();
            }

            ViewData["AppUserId"] = new SelectList(await GetClubMembers(), "Id", "FirstName", personInWorkout.AppUserId);
            ViewData["WorkOutId"] = new SelectList(await _uow.Workout.GetAllAsync(), "Id", "Id", personInWorkout.WorkOutId);
            return View(personInWorkout);
        }

        // POST: Admin/PersonInWorkout/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("WorkOutId,AppUserId,Comment,Id")] PersonInWorkout personInWorkout)
        {
            if (id != personInWorkout.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.PersonInWorkout.Update(personInWorkout);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _uow.PersonInWorkout.ExistsAsync(personInWorkout.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["AppUserId"] = new SelectList(await GetClubMembers(), "Id", "FirstName", personInWorkout.AppUserId);
            ViewData["WorkOutId"] = new SelectList(await _uow.Workout.GetAllAsync(), "Id", "Id", personInWorkout.WorkOutId);
            return View(personInWorkout);
        }

        // GET: Admin/PersonInWorkout/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInWorkout = await _uow.PersonInWorkout.FirstOrDefaultAsync(id.Value, User.GetUserId());


            return personInWorkout == null ? NotFound() : View(personInWorkout);
        }

        // POST: Admin/PersonInWorkout/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.PersonInWorkout.RemoveAsync(id);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<AppUser>> GetClubMembers()
        {
            var userId = User.GetUserId();
            var coachClubId = (await _uow.PersonInClub.GetAllUserClubs(userId));
            var clubPlayers = (await _uow.Users.GetAllClubPlayersByCoachClubId(coachClubId)).ToList();

            return clubPlayers;
        }
    }
}