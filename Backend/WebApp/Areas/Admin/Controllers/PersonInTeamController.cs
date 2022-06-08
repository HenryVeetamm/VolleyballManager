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
    public class PersonInTeamController : Controller
    {
        
        private readonly IAppUnitOfWork _uow;

        public PersonInTeamController(IAppUnitOfWork uow)
        {
            _uow = uow;
        
        }

        // GET: Admin/PersonInTeam
        public async Task<IActionResult> Index()
        {   //Coach can see who belongs to each team when clicked on details
            //Player can see in which team HE/SHE belongs.
            
            return View((await _uow.PersonInTeam.GetAllAsync(User.GetUserId())).ToList());
        }

        // GET: Admin/PersonInTeam/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInTeams = await _uow.PersonInTeam.GetAllPersonInTeamByTeamId(id.Value);


            return !personInTeams.Any() ? NotFound() : View(personInTeams);
            
        }

        // GET: Admin/PersonInTeam/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AppUserId"] = new SelectList(await GetClubMembers(), "Id", "FirstName");
            ViewData["RolesInTeamId"] = new SelectList(await _uow.RolesInTeam.GetAllAsync(), "Id", "RoleDescription");
            ViewData["TeamId"] = new SelectList(await GetClubTeams(), "Id", "Code");
            return View();
        }

        // POST: Admin/PersonInTeam/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppUserId,TeamId,RolesInTeamId,Id")] PersonInTeam personInTeam)
        {
            if (ModelState.IsValid)
            {
                _uow.PersonInTeam.Add(personInTeam);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(await GetClubMembers(), "Id", "FirstName");
            ViewData["RolesInTeamId"] = new SelectList(await _uow.RolesInTeam.GetAllAsync(), "Id", "RoleDescription");
            ViewData["TeamId"] = new SelectList(await GetClubTeams(), "Id", "Code");
            return View(personInTeam);
        }

        // GET: Admin/PersonInTeam/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInTeam = await _uow.PersonInTeam.FirstOrDefaultAsync(id.Value);
            
            if (personInTeam == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(await GetClubMembers(), "Id", "FirstName");
            ViewData["RolesInTeamId"] = new SelectList(await _uow.RolesInTeam.GetAllAsync(), "Id", "RoleDescription");
            ViewData["TeamId"] = new SelectList(await GetClubTeams(), "Id", "Code");
            return View(personInTeam);
        }

        // POST: Admin/PersonInTeam/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AppUserId,TeamId,RolesInTeamId,Id")] PersonInTeam personInTeam)
        {
            if (id != personInTeam.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.PersonInTeam.Update(personInTeam);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _uow.PersonInTeam.ExistsAsync(personInTeam.Id))
                    {
                        return NotFound();
                    }
                    throw;
                    
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(await GetClubMembers(), "Id", "FirstName");
            ViewData["RolesInTeamId"] = new SelectList(await _uow.RolesInTeam.GetAllAsync(), "Id", "RoleDescription");
            ViewData["TeamId"] = new SelectList((await GetClubTeams()).ToList(), "Id", "Code");
            return View(personInTeam);
        }

        // GET: Admin/PersonInTeam/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInTeam = await _uow.PersonInTeam.FirstOrDefaultAsync(id.Value);
            
            if (personInTeam == null)
            {
                return NotFound();
            }

            return View(personInTeam);
        }

        // POST: Admin/PersonInTeam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.PersonInTeam.RemoveAsync(id);
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
        
        private async Task<IEnumerable<Team?>> GetClubTeams()
        {
            var userId = User.GetUserId();
            var clubTeams = (await _uow.Team.GetAllPersonTeamsByUserId(userId)).ToList();

            return clubTeams;
        }
    }
}
