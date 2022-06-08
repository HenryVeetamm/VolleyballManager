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
    public class ClubController : Controller
    {
        private readonly IAppUnitOfWork _uow;


        public ClubController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Admin/Club
        public async Task<IActionResult> Index()
        {
            return View(await _uow.Club.GetUserOwnedClubs(User.GetUserId()));
        }

        // GET: Admin/Club/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _uow.Club
                .FirstOrDefaultAsync(id.Value);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // GET: Admin/Club/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Club/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id")] Club club)
        {
            if (ModelState.IsValid)
            {
                club.Id = Guid.NewGuid();
                club.AppUserId = User.GetUserId();
                _uow.Club.Add(club);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(club);
        }

        // GET: Admin/Club/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _uow.Club.FirstOrDefaultAsync(id.Value);
            if (club == null)
            {
                return NotFound();
            }
            return View(club);
        }

        // POST: Admin/Club/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Id")] Club club)
        {
            if (id != club.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    club.AppUserId = User.GetUserId();
                    _uow.Club.Update(club);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _uow.Club.ExistsAsync(club.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(club);
        }

        // GET: Admin/Club/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) { return NotFound(); }

            var club = await _uow.Club.FirstOrDefaultAsync(id.Value);

            return club == null ? NotFound() : View(club);
        }

        // POST: Admin/Club/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.Club.RemoveAsync(id);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
