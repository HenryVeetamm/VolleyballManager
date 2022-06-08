#nullable disable

using App.DAL.Contracts;
using App.DAL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Base.Extension;
using DAL.App.EF;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AnnouncementController : Controller
    {
        
        private readonly IAppUnitOfWork _uow;

        public AnnouncementController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Admin/Announcement
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Player"))
            {
                var playerClubIds = (await _uow.PersonInClub.GetAllUserClubs(User.GetUserId())).ToList();
                
                var allAnnouncements = await _uow.Announcement.GetAllAnnouncementsByClubId(playerClubIds);
                
                var annons = await _uow.Announcement.GetAllPlayerAnnouncementsByUserId(User.GetUserId());
                annons = annons.Concat(allAnnouncements).ToList();
                
                return View(annons.OrderBy(x => !x.Pinned).ToList());
            }

            if (User.IsInRole("Coach"))
            {
                return View((await _uow.Announcement.GetAllAsync(User.GetUserId())).ToList());
            }
            
            return View();
        }

        // GET: Admin/Announcement/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var announcement = await _uow.Announcement.FirstOrDefaultAsync(id.Value);

            return announcement == null ? NotFound() : View(announcement);
        }

        // GET: Admin/Announcement/Create
        public  async Task<IActionResult> Create()
        {
            ViewData["TeamId"] = new SelectList((await _uow.Team.GetAllPersonTeamsByUserId(User.GetUserId())).ToList()
                , "Id", "Code");
            return View();
        }

        // POST: Admin/Announcement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppUserId,TeamId,Title,Content,Pinned,Id")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                announcement.AppUserId = User.GetUserId();
                _uow.Announcement.Add(announcement);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TeamId"] = await GetUserTeams(User.GetUserId(), announcement);
            
            return View(announcement);
        }

        // GET: Admin/Announcement/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _uow.Announcement.FirstOrDefaultAsync(id.Value);
            if (announcement == null)
            {
                return NotFound();
            }
            
            
            ViewData["TeamId"] = await GetUserTeams(User.GetUserId(), announcement);
            return View(announcement);
        }

        // POST: Admin/Announcement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AppUserId,TeamId,Title,Content,Pinned,Id")] Announcement announcement)
        {
            if (id != announcement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    announcement.AppUserId = User.GetUserId();
                    _uow.Announcement.Update(announcement);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _uow.Announcement.ExistsAsync(announcement.Id))
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
           
            ViewData["TeamId"] = GetUserTeams(User.GetUserId(), announcement);
            return View(announcement);
        }

        // GET: Admin/Announcement/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _uow.Announcement.FirstOrDefaultAsync(id.Value);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // POST: Admin/Announcement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
           
            await _uow.Announcement.RemoveAsync(id);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        

        private async Task<SelectList> GetUserTeams(Guid userId, Announcement announcement)
        {
            return new SelectList((await _uow.Team.GetAllPersonTeamsByUserId(userId)).ToList()
                , "Id", "Code", announcement.TeamId);
        }
    }
}
