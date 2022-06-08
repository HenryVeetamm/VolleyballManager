#nullable disable

using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.DTO.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


using Base.Extension;
using DAL.App.EF;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PersonInMatchController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public PersonInMatchController(IAppUnitOfWork uow)
        {
            _uow = uow;
       
        }

        // GET: Admin/PersonInMatch
        public async Task<IActionResult> Index()
        {   
            
            //Coach and players can see all personnel in a match.
            
            var userId = User.GetUserId();
            return View(await _uow.PersonInMatch.GetUserPersonInMatches(userId));
            /*return View(await _uow.PersonInMatch.GetAllAsync(userId));*/
        }

        // GET: Admin/PersonInMatch/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInMatch = await _uow.PersonInMatch.GetAllPersonInMatchByMatchId(id.Value);
            if (personInMatch == null)
            {
                return NotFound();
            }

            return View(personInMatch);
            
            
        }

        // GET: Admin/PersonInMatch/Create
        public async  Task<IActionResult> Create()
        {
            ViewData["AppUserId"] = new SelectList(await GetClubMembers(), "Id", "FirstName");
            ViewData["MatchId"] = new SelectList(await _uow.Match.GetAllAsync(User.GetUserId()), "Id", "MatchScore");
            return View();
        }

        // POST: Admin/PersonInMatch/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppUserId,MatchId,TotalPoints,Aces,Faults,Reception,Id")] PersonInMatch personInMatch)
        {
            if (ModelState.IsValid)
            {
                personInMatch.Id = Guid.NewGuid();
                _uow.PersonInMatch.Add(personInMatch);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["AppUserId"] = new SelectList(await GetClubMembers(), "Id", "FirstName");
            ViewData["MatchId"] = new SelectList(await _uow.Match.GetAllAsync(User.GetUserId()), "Id", "MatchScore");
            return View(personInMatch);
        }

        // GET: Admin/PersonInMatch/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInMatch = await _uow.PersonInMatch.FirstOrDefaultAsync(id.Value, User.GetUserId());
            if (personInMatch == null)
            {
                return NotFound();
            }
            
            ViewData["AppUserId"] = new SelectList(await GetClubMembers(), "Id", "FirstName");
            ViewData["MatchId"] = new SelectList(await _uow.Match.GetAllAsync(User.GetUserId()), "Id", "MatchScore");
            return View(personInMatch);
        }

        // POST: Admin/PersonInMatch/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AppUserId,MatchId,TotalPoints,Aces,Faults,Reception,Id")] PersonInMatch personInMatch)
        {
            if (id != personInMatch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.PersonInMatch.Update(personInMatch);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _uow.PersonInMatch.ExistsAsync(personInMatch.Id))
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
            
            
            ViewData["AppUserId"] = new SelectList(await GetClubMembers(), "Id", "FirstName");
            ViewData["MatchId"] = new SelectList(await _uow.Match.GetAllAsync(User.GetUserId()), "Id", "MatchScore");
            return View(personInMatch);
        }

        // GET: Admin/PersonInMatch/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInMatch = await _uow.PersonInMatch.FirstOrDefaultAsync(id.Value);
        
            if (personInMatch == null)
            {
                return NotFound();
            }

            return View(personInMatch);
        }

        // POST: Admin/PersonInMatch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var personInMatch = await _uow.PersonInMatch.FirstOrDefaultAsync(id);
            await _uow.PersonInMatch.RemoveAsync(personInMatch!.Id);
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
