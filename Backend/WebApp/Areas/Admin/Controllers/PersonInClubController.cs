#nullable disable

using App.DAL.Contracts;
using App.DAL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Domain.Identity;
using Base.Extension;

using Microsoft.AspNetCore.Identity;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PersonInClubController : Controller
    {
        private readonly IAppUnitOfWork _uow;
        private readonly UserManager<AppUser> _userManager;

        public PersonInClubController(IAppUnitOfWork uow, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _uow = uow;
        }

        // GET: Admin/PersonInClub
        public async Task<IActionResult> Index()
        {
            return View((await _uow.PersonInClub.GetAllUserClubs(User.GetUserId())).ToList());
        }

        // GET: Admin/PersonInClub/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInClub = await _uow.PersonInClub.FirstOrDefaultAsync(id.Value, User.GetUserId());

            return personInClub != null ? View(personInClub) : NotFound();
        }

        // GET: Admin/PersonInClub/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AppUserId"] = new SelectList(await _uow.Users.GetAllAsync(), "Id", "FirstName");
            ViewData["ClubId"] = new SelectList(await _uow.Club.GetAllAsync(), "Id", "Name");
            /*ViewData["ClubId"] = new SelectList(await _uow.Club.GetUserClubs(User.GetUserId()), "Id", "Name");*/
            ViewData["Role"] = new SelectList(new List<string>() { "Player", "Coach" }, "RoleValue");
            return View();
        }

        // POST: Admin/PersonInClub/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClubId,AppUserId,Id")] PersonInClub personInClub)
        {
            if (ModelState.IsValid)
            {
                personInClub.Id = Guid.NewGuid();
                _uow.PersonInClub.Add(personInClub);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AppUserId"] =
                new SelectList(await _uow.Users.GetAllAsync(), "Id", "FirstName", personInClub.AppUserId);
            ViewData["ClubId"] = new SelectList(await _uow.Club.GetAllAsync(), "Id", "Name", personInClub.ClubId);
            return View(personInClub);
        }

        // GET: Admin/PersonInClub/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInClub = await _uow.PersonInClub.FirstOrDefaultAsync(id.Value, User.GetUserId());
            if (personInClub == null)
            {
                return NotFound();
            }

            ViewData["AppUserId"] =
                new SelectList(await _uow.Users.GetAllAsync(), "Id", "FirstName", personInClub.AppUserId);
            ViewData["ClubId"] = new SelectList(await _uow.Club.GetAllAsync(), "Id", "Name", personInClub.ClubId);
            return View(personInClub);
        }

        // POST: Admin/PersonInClub/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ClubId,AppUserId,Id")] PersonInClub personInClub)
        {
            if (id != personInClub.Id)
            {
                return NotFound();
            }

            var personInClubFromUow = await _uow.PersonInClub.FirstOrDefaultAsync(id /*User.GetUserId()*/);

            if (personInClubFromUow == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.PersonInClub.Update(personInClub);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _uow.PersonInClub.ExistsAsync(personInClub.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["AppUserId"] =
                new SelectList(await _uow.Users.GetAllAsync(), "Id", "FirstName", personInClub.AppUserId);
            ViewData["ClubId"] = new SelectList(await _uow.Club.GetAllAsync(), "Id", "Name", personInClub.ClubId);
            return View(personInClub);
        }

        // GET: Admin/PersonInClub/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInClub = await _uow.PersonInClub.FirstOrDefaultAsync(id.Value, User.GetUserId());

            return personInClub != null ? View(personInClub) : NotFound();
        }

        // POST: Admin/PersonInClub/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.PersonInClub.RemoveAsync(id);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}