using Microsoft.AspNetCore.Mvc;
using Project_1.Dtos.HRDtos;
using Project_1.Services.Base;

namespace Project_1.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(
            IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_departmentService.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateDepartmentDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateDepartmentDto dto)
        {
            ValidateName(dto);

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            _departmentService.Create(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var department = _departmentService.GetById(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CreateDepartmentDto dto)
        {
            var department = _departmentService.GetById(id);

            if (department == null)
            {
                return NotFound();
            }

            ValidateName(dto);

            if (!ModelState.IsValid)
            {
                department.Name = dto.Name;

                return View(department);
            }

            if (!_departmentService.Update(id, dto))
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (!_departmentService.Delete(id))
                {
                    return NotFound();
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/api/Department")]
        public IActionResult GetAll()
        {
            return Ok(_departmentService.GetAll());
        }

        [HttpGet("/api/Department/{id:int}")]
        public IActionResult GetById(int id)
        {
            var department = _departmentService.GetById(id);

            if (department == null)
            {
                return NotFound("Department not found");
            }

            return Ok(department);
        }

        private void ValidateName(CreateDepartmentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                ModelState.AddModelError(
                    nameof(dto.Name),
                    "اكتب اسم القسم.");
            }
        }
    }
}