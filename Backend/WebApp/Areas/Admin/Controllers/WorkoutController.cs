#nullable disable
using App.DAL.Contracts;
using App.DAL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Base.Extension;


namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WorkoutController : Controller
    {
        private readonly IAppUnitOfWork _uow;
        
        public WorkoutController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Admin/Workout
        public async Task<IActionResult> Index()
        {
            return View(await _uow.Workout.GetAllAsync());
        }

        // GET: Admin/Workout/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await _uow.Workout.FirstOrDefaultAsync(id.Value);
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // GET: Admin/Workout/Create
        public async Task<IActionResult> Create()
        {
            ViewData["WorkoutTypeId"] = new SelectList(await _uow.WorkoutType.GetAllAsync(), "Id", "Description");
            return View();
        }

        // POST: Admin/Workout/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkoutTypeId,Description,Date,Id")] Workout workout)
        {
            if (ModelState.IsValid)
            {
                workout.AppUserId = User.GetUserId();
                var work = _uow.Workout.Add(workout);
                var userId = User.GetUserId();
                
                var personInWorkout = new PersonInWorkout()
                {
                    WorkOutId = work.Id,
                    AppUserId = userId
                };
                var afterAdd = _uow.PersonInWorkout.Add(personInWorkout);
               
                await _uow.SaveChangesAsync();
                /*return RedirectToAction("Details", "PersonInWorkout", new {id=afterAdd.Id});*/
                return RedirectToAction(nameof(Index));
            }

            ViewData["WorkoutTypeId"] =
                new SelectList(await _uow.WorkoutType.GetAllAsync(), "Id", "Description", workout.WorkoutTypeId);
            return View(workout);
        }

        // GET: Admin/Workout/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await _uow.Workout.FirstOrDefaultAsync(id.Value);
            if (workout == null)
            {
                return NotFound();
            }

            ViewData["WorkoutTypeId"] =
                new SelectList(await _uow.WorkoutType.GetAllAsync(), "Id", "Description", workout.WorkoutTypeId);
            return View(workout);
        }
        

        // POST: Admin/Workout/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("WorkoutTypeId,Description,Date,Id")] Workout workout)
        {
            if (id != workout.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    workout.AppUserId = User.GetUserId();
                    _uow.Workout.Update(workout);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _uow.Workout.ExistsAsync(workout.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["WorkoutTypeId"] =
                new SelectList(await _uow.WorkoutType.GetAllAsync(), "Id", "Description", workout.WorkoutTypeId);
            return View(workout);
        }

        // GET: Admin/Workout/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await _uow.Workout.FirstOrDefaultAsync(id.Value);
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // POST: Admin/Workout/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            await _uow.Workout.RemoveAsync(id);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> WorkoutExists(Guid id)
        {
            return await _uow.Workout.ExistsAsync(id);
        }
    }
}