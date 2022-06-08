#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.DAL.Contracts;
using App.DAL.DTO;
using App.Domain.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Base.Extension;
using DAL.App.EF;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SavedComparisonController : Controller
    {
        private readonly IAppUnitOfWork _uow;
        private readonly UserManager<AppUser> _userManager;

        public SavedComparisonController(IAppUnitOfWork uow, UserManager<AppUser> userManager)
        {
            _uow = uow;
            _userManager = userManager;
        }

        // GET: Admin/SavedComparison
        public async Task<IActionResult> Index()
        {
            var userId = User.GetUserId();
            return View((await _uow.SavedComparison.GetPlayerComparisonsByUserId(userId)).ToList());
        }

        // GET: Admin/SavedComparison/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var savedComparison = await _uow.SavedComparison.FirstOrDefaultAsync(id.Value, User.GetUserId());
            
            if (savedComparison == null)
            {
                return NotFound();
            }

            return View(savedComparison);
        }

        // GET: Admin/SavedComparison/Create
        public async Task<IActionResult> Create()
        {

            ViewData["ComparableId"] = new SelectList(await GetPlayersList(), "Id", "FirstName");
            return View();
        }

        // POST: Admin/SavedComparison/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ComparerId,ComparableId,Id")] SavedComparison savedComparison)
        {
            if (ModelState.IsValid)
            {
                savedComparison.Id = Guid.NewGuid();
                savedComparison.ComparerId = User.GetUserId();
                _uow.SavedComparison.Add(savedComparison);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["ComparableId"] = new SelectList(await GetPlayersList(), "Id", "FirstName", savedComparison.ComparableId);
            return View(savedComparison);
        }

        // GET: Admin/SavedComparison/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var savedComparison = await _uow.SavedComparison.FirstOrDefaultAsync(id.Value, User.GetUserId());
            if (savedComparison == null)
            {
                return NotFound();
            }
            ViewData["ComparableId"] = new SelectList(await GetPlayersList(), "Id", "FirstName", savedComparison.ComparableId);
            return View(savedComparison);
        }

        // POST: Admin/SavedComparison/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ComparerId,ComparableId,Id")] SavedComparison savedComparison)
        {
            
            
            if (id != savedComparison.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    savedComparison.ComparerId = User.GetUserId();
                    _uow.SavedComparison.Update(savedComparison);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _uow.SavedComparison.ExistsAsync(savedComparison.Id))
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
            
            ViewData["ComparableId"] = new SelectList(await GetPlayersList(), "Id", "FirstName", savedComparison.ComparableId);
            return View(savedComparison);
        }

        // GET: Admin/SavedComparison/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var savedComparison = await _uow.SavedComparison.FirstOrDefaultAsync(id.Value, User.GetUserId());
            if (savedComparison == null)
            {
                return NotFound();
            }

            return View(savedComparison);
        }

        // POST: Admin/SavedComparison/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            
            await _uow.SavedComparison.RemoveAsync(id);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       

        private async Task<IList<AppUser>> GetPlayersList()
        {
            var playerOnly = (await _userManager.GetUsersInRoleAsync("Player")).ToList();
            var userId = User.GetUserId();
            return playerOnly.Where(a => a.Id != userId).ToList();
        }
    }
}
