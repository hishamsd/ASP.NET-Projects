using Microsoft.AspNetCore.Mvc;
using Project_1.Domain.Models;
using Project_1.Application.Services.Base;

namespace Project_1.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;
        private readonly ICategoryService _categoryService;

        public PlanController(IPlanService planService, ICategoryService categoryService)
        {
            _planService = planService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_planService.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadCategories();
            return View(new Plan());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Plan plan, IFormFile? image)
        {
            ValidatePlan(plan);
            LoadCategories(plan.CategoryId);
            if (!ModelState.IsValid)
            {
                return View(plan);
            }

            try
            {
                _planService.Create(plan, image);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("image", ex.Message);
                return View(plan);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var plan = _planService.GetById(id);

            if (plan == null)
            {
                return NotFound();
            }

            LoadCategories(plan.CategoryId);
            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Plan plan, IFormFile? image)
        {
            plan.ImageURL = _planService.GetById(plan.Id)?.ImageURL;
            ValidatePlan(plan);
            LoadCategories(plan.CategoryId);
            if (!ModelState.IsValid)
            {
                return View(plan);
            }

            try
            {
                if (!_planService.Update(plan, image))
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("image", ex.Message);
                plan.ImageURL = _planService.GetById(plan.Id)?.ImageURL;
                return View(plan);
            }
        }

        private void LoadCategories(int? selected = null)
        {
            ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _categoryService.GetCategories(), "Id", "Name", selected);
        }

        private void ValidatePlan(Plan plan)
        {
            if (plan.CategoryId.HasValue && !_categoryService.GetCategories().Any(category => category.Id == plan.CategoryId))
                ModelState.AddModelError(nameof(plan.CategoryId), "اختر فئة موجودة.");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var plan = _planService.GetById(id);

            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Plan plan)
        {
            if (!_planService.Delete(plan.Id))
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}