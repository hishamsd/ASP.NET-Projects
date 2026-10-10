using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_1.Application.Dtos.HRDtos;
using Project_1.Application.Services.Base;

namespace Project_1.Controllers
{
    public class JobController : Controller
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_jobService.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadDepartments();

            return View(new CreateJobDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateJobDto dto)
        {
            ValidateInput(dto);

            if (ModelState.IsValid)
            {
                try
                {
                    _jobService.Create(dto);

                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(
                        nameof(dto.DepartmentId),
                        ex.Message);
                }
            }

            LoadDepartments(dto.DepartmentId);

            return View(dto);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var job = _jobService.GetById(id);

            if (job == null)
            {
                return NotFound();
            }

            LoadDepartments(job.DepartmentId);

            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CreateJobDto dto)
        {
            var job = _jobService.GetById(id);

            if (job == null)
            {
                return NotFound();
            }

            ValidateInput(dto);

            if (ModelState.IsValid)
            {
                try
                {
                    if (!_jobService.Update(id, dto))
                    {
                        return NotFound();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(
                        nameof(dto.DepartmentId),
                        ex.Message);
                }
            }

            job.Name = dto.Name;
            job.DepartmentId = dto.DepartmentId;

            LoadDepartments(dto.DepartmentId);

            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (!_jobService.Delete(id))
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

        [HttpGet("/api/Job")]
        public IActionResult GetAll()
        {
            return Ok(_jobService.GetAll());
        }

        [HttpGet("/api/Job/{id:int}")]
        public IActionResult GetById(int id)
        {
            var job = _jobService.GetById(id);

            if (job == null)
            {
                return NotFound("Job not found");
            }

            return Ok(job);
        }

        private void LoadDepartments(int? departmentId = null)
        {
            ViewBag.Departments = new SelectList(
                _jobService.GetAllDepts(),
                "Id",
                "Name",
                departmentId);
        }

        private void ValidateInput(CreateJobDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                ModelState.AddModelError(
                    nameof(dto.Name),
                    "اكتب اسم الوظيفة.");
            }

            if (dto.DepartmentId <= 0)
            {
                ModelState.AddModelError(
                    nameof(dto.DepartmentId),
                    "اختر القسم.");
            }
        }
    }
}