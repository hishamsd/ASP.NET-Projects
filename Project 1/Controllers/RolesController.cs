using Microsoft.AspNetCore.Mvc;
using Project_1.Services.Base;

namespace Project_1.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_roleService.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("Name", "اكتب اسم الرول.");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", _roleService.GetAll());
            }

            _roleService.Create(name);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("Name", "اكتب اسم الرول.");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", _roleService.GetAll());
            }

            var updated = _roleService.Update(id, name);

            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var deleted = _roleService.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult AssignPermissions(int roleId)
        {
            var role = _roleService.GetByIdWithPermissions(roleId);

            if (role == null)
            {
                return NotFound();
            }

            ViewBag.AllPermissions =
                _roleService.GetAllPermissions().ToList();

            ViewBag.AssignedPermissions = role.Permissions
                .Select(permission => permission.Id)
                .ToList();

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AssignPermissions(
            int roleId,
            List<int>? permissionIds)
        {
            var saved = _roleService.AssignPermissions(
                roleId,
                permissionIds ?? new List<int>());

            if (!saved)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}