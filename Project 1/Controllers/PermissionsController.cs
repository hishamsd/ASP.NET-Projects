using Microsoft.AspNetCore.Mvc;
using Project_1.Application.Dtos;
using Project_1.Application.Services.Base;

namespace Project_1.Controllers
{
    public class PermissionsController : Controller
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(
            IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public IActionResult Index()
        {
            return View(_permissionService.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatePermissionDto permissionDto)
        {
            if (string.IsNullOrWhiteSpace(permissionDto.Name))
            {
                ModelState.AddModelError("Name", "اكتب اسم الصلاحية.");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", _permissionService.GetAll());
            }

            _permissionService.Create(permissionDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UpdatePermissionDto permissionDto)
        {
            if (string.IsNullOrWhiteSpace(permissionDto.Name))
            {
                ModelState.AddModelError("Name", "اكتب اسم الصلاحية.");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", _permissionService.GetAll());
            }

            var updated = _permissionService.Update(permissionDto);

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
            var deleted = _permissionService.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}