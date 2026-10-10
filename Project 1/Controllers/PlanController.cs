using Microsoft.AspNetCore.Mvc;
using Project_1.Models;
using Project_1.Services.Base;

namespace Project_1.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_planService.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Plan());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Plan plan)
        {
            if (!ModelState.IsValid)
            {
                return View(plan);
            }

            _planService.Create(plan);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
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
        public IActionResult Edit(Plan plan)
        {
            if (!ModelState.IsValid)
            {
                return View(plan);
            }

            if (!_planService.Update(plan))
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
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