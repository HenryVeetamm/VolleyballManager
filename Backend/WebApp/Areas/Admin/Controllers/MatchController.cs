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
    public class MatchController : Controller
    {
       
        private readonly IAppUnitOfWork _uow;
        public MatchController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Admin/Match
        public async Task<IActionResult> Index()
        {
           
            return View((await _uow.Match.GetAllAsync(User.GetUserId())).ToList());
        }

        // GET: Admin/Match/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _uow.Match.FirstOrDefaultAsync(id.Value);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // GET: Admin/Match/Create
        public async Task<IActionResult> Create()
        {
            
            var userTeams = await _uow.Team.GetAllPersonTeamsByUserId(User.GetUserId());
            ViewData["AwayTeamId"] = new SelectList(await _uow.Team.GetAllOpponentTeams(User.GetUserId()), "Id", "Code");
            ViewData["HomeTeamId"] = new SelectList(userTeams, "Id", "Code");
            return View();
        }

        // POST: Admin/Match/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HomeTeamId,AwayTeamId,MatchDate,MatchScore,Id")] Match match)
        {
            if (ModelState.IsValid)
            {
                match.Id = Guid.NewGuid();
                match.AppUserId = User.GetUserId();
                _uow.Match.Add(match);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AwayTeamId"] = new SelectList(await _uow.Team.GetAllAsync(), "Id", "Code", match.AwayTeamId);
            ViewData["HomeTeamId"] = new SelectList(await _uow.Team.GetAllAsync(), "Id", "Code", match.HomeTeamId);
            return View(match);
        }

        // GET: Admin/Match/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _uow.Match.FirstOrDefaultAsync(id.Value);
            if (match == null)
            {
                return NotFound();
            }
            ViewData["AwayTeamId"] = new SelectList(await _uow.Team.GetAllAsync(), "Id", "Code", match.AwayTeamId);
            ViewData["HomeTeamId"] = new SelectList(await _uow.Team.GetAllAsync(), "Id", "Code", match.HomeTeamId);
            return View(match);
        }

        // POST: Admin/Match/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("HomeTeamId,AwayTeamId,MatchDate,MatchScore,Id")] Match match)
        {
            if (id != match.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    match.AppUserId = User.GetUserId();
                    _uow.Match.Update(match);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _uow.Match.ExistsAsync(match.Id))
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
            
            ViewData["AwayTeamId"] = new SelectList(await _uow.Team.GetAllAsync(), "Id", "Code", match.AwayTeamId);
            ViewData["HomeTeamId"] = new SelectList(await _uow.Team.GetAllAsync(), "Id", "Code", match.HomeTeamId);
            return View(match);
        }

        // GET: Admin/Match/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _uow.Match.FirstOrDefaultAsync(id.Value);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // POST: Admin/Match/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            
            await _uow.Match.RemoveAsync(id);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
