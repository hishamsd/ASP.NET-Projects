using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_1.Dtos.HRDtos;
using Project_1.Services.Base;

namespace Project_1.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employees = _employeeService.GetEmployeesImprove();

            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadDropdowns();

            return View(new CreateEmployeeDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateEmployeeDto employeeDto)
        {
            if (employeeDto.Image == null ||
                employeeDto.Image.Length == 0)
            {
                ModelState.AddModelError(
                    nameof(CreateEmployeeDto.Image),
                    "لازم تختار صورة الموظف.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _employeeService.CreateEmployee(employeeDto);

                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(
                        nameof(CreateEmployeeDto.Image),
                        ex.Message);
                }
            }

            LoadDropdowns(
                employeeDto.JobId,
                employeeDto.DepartmentId);

            return View(employeeDto);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _employeeService.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound();
            }

            var employeeDto = new UpdateEmployeeDto
            {
                Id = employee.Id,
                UID = employee.UID,
                Name = employee.Name,
                Email = employee.Email,
                Phone = employee.Phone,
                JobId = employee.JobId,
                DepartmentId = employee.DepartmentId
            };

            LoadDropdowns(
                employee.JobId,
                employee.DepartmentId);

            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UpdateEmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updated = _employeeService.UpdateEmployee(
                        employeeDto.Id,
                        employeeDto);

                    if (!updated)
                    {
                        return NotFound();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(
                        nameof(CreateEmployeeDto.Image),
                        ex.Message);
                }
            }

            LoadDropdowns(
                employeeDto.JobId,
                employeeDto.DepartmentId);

            return View(employeeDto);
        }

        private void LoadDropdowns(
            int? jobId = null,
            int? departmentId = null)
        {
            ViewBag.Jobs = new SelectList(
                _employeeService.GetAllJobs(),
                "Id",
                "Name",
                jobId);

            ViewBag.Departments = new SelectList(
                _employeeService.GetAllDepts(),
                "Id",
                "Name",
                departmentId);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var deleted = _employeeService.DeleteEmployee(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}