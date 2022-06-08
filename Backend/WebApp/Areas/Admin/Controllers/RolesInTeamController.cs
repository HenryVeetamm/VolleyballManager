#nullable disable

using App.DAL.Contracts;
using App.DAL.DTO;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using Base.Domain;

using WebApp.DTO;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesInTeamController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public RolesInTeamController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Admin/RolesInTeam
        public async Task<IActionResult> Index()
        {
            return View(await _uow.RolesInTeam.GetAllAsync());
        }

        // GET: Admin/RolesInTeam/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rolesInTeam = await _uow.RolesInTeam.FirstOrDefaultAsync(id.Value);
            
            if (rolesInTeam == null)
            {
                return NotFound();
            }

            return View(rolesInTeam);
        }

        // GET: Admin/RolesInTeam/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/RolesInTeam/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleDescription,Id")] RolesInTeamDTO rolesInTeamDto)
        {
            if (ModelState.IsValid)
            {
                var rolesInTeam = new RolesInTeam()
                {
                    Id = new Guid(),
                    RoleDescription = new LangStr(rolesInTeamDto.RoleDescription)
                };
                _uow.RolesInTeam.Add(rolesInTeam);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rolesInTeamDto);
        }

        // GET: Admin/RolesInTeam/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rolesInTeam = await _uow.RolesInTeam.FirstOrDefaultAsync(id.Value);
            if (rolesInTeam == null)
            {
                return NotFound();
            }
            return View(rolesInTeam);
        }

        // POST: Admin/RolesInTeam/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("RoleDescription,Id")] string roleDescription)
        {   
            var roleInTeam = await _uow.RolesInTeam.FirstOrDefaultAsync(id);
            if (roleInTeam == null )
            {
                return NotFound();
            }

           
            roleInTeam.RoleDescription.SetTranslation(roleDescription);

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.RolesInTeam.Update(roleInTeam);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _uow.RolesInTeam.ExistsAsync(roleInTeam.Id))
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
            return View(roleInTeam);
        }

        // GET: Admin/RolesInTeam/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rolesInTeam = await _uow.RolesInTeam.FirstOrDefaultAsync(id.Value);
            if (rolesInTeam == null)
            {
                return NotFound();
            }

            return View(rolesInTeam);
        }

        // POST: Admin/RolesInTeam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.RolesInTeam.RemoveAsync(id);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
