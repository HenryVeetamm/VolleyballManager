#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.DAL.Contracts;
using App.DAL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Base.Domain;
using WebApp.DTO;
using DAL.App.EF;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WorkoutTypeController : Controller
    {
     
        private readonly IAppUnitOfWork _uow;

        public WorkoutTypeController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Admin/WorkoutType
        public async Task<IActionResult> Index()
        {
            var res1 = await _uow.WorkoutType.GetAllAsync();
            
            var res = res1
                .Select(x => new WorkoutTypeDTO()
            {
                Id = x.Id,
                Description = x.Description
            }).ToList();
            return View(res1);
        }

        // GET: Admin/WorkoutType/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) { return NotFound(); }

            var workoutType = await _uow.WorkoutType.FirstOrDefaultAsync(id.Value);

            return workoutType == null ? NotFound() : View(workoutType);
        }

        // GET: Admin/WorkoutType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/WorkoutType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,Id")] string Description)
        {
            var workoutType = new WorkoutType()
            {
                Description = new LangStr(Description)
            };
            
            if (ModelState.IsValid)
            {
                workoutType.Id = Guid.NewGuid();
                _uow.WorkoutType.Add(workoutType);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workoutType);
        }

        // GET: Admin/WorkoutType/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) { return NotFound(); }

            var workoutType = await _uow.WorkoutType.FirstOrDefaultAsync(id.Value);
            if (workoutType == null)
            {
                return NotFound();
            }
            return View(workoutType);
        }

        // POST: Admin/WorkoutType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Description,Id")] string Description)
        {
            
            var workoutType = await _uow.WorkoutType.FirstOrDefaultAsync(id);
            if (workoutType == null)
            {
                return NotFound();
            }

            workoutType.Description.SetTranslation(Description);
            
            if (id != workoutType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.WorkoutType.Update(workoutType);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _uow.WorkoutType.ExistsAsync(workoutType.Id))
                    {
                        return NotFound();
                    }
                    
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(workoutType);
        }

        // GET: Admin/WorkoutType/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutType = await _uow.WorkoutType
                .FirstOrDefaultAsync(id.Value);
            if (workoutType == null)
            {
                return NotFound();
            }

            return View(workoutType);
        }

        // POST: Admin/WorkoutType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.WorkoutType.RemoveAsync(id);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
