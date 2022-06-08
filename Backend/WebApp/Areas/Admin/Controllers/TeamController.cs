#nullable disable

using App.DAL.Contracts;
using App.DAL.DTO;
using Base.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using DAL.App.EF;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
       
        private readonly IAppUnitOfWork _uow;

        public TeamController(IAppUnitOfWork uow)
        {
           
            _uow = uow;
        }

        // GET: Admin/Team
        public async Task<IActionResult> Index()
        {
            
            return View(await _uow.Team.GetAllAsync());
        }

        // GET: Admin/Team/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _uow.Team.FirstOrDefaultAsync(id.Value);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Admin/Team/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ClubId"] = await ClubsSelectList();
            return View();
        }

        // POST: Admin/Team/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClubId,Name,Code,Id")] Team team)
        {
            if (ModelState.IsValid)
            {
                team.Id = Guid.NewGuid();
                _uow.Team.Add(team);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(team);
        }

        // GET: Admin/Team/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _uow.Team.FirstOrDefaultAsync(id.Value);
            if (team == null)
            {
                return NotFound();
            }

            ViewData["ClubId"] = await ClubsSelectList();
            return View(team);
        }

        // POST: Admin/Team/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ClubId,Name,Code,Id")] Team team)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.Team.Update(team);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _uow.Team.ExistsAsync(team.Id))
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
            ViewData["ClubId"] = await ClubsSelectList();
            return View(team);
        }

        // GET: Admin/Team/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _uow.Team.FirstOrDefaultAsync(id.Value);
                
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Admin/Team/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.Team.RemoveAsync(id);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        

        private async Task<SelectList> ClubsSelectList()
        {
            return new SelectList(await _uow.Club.GetUserOwnedClubs(User.GetUserId()), "Id", "Name");
        }
    }
}
