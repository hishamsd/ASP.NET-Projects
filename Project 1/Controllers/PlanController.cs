using Microsoft.AspNetCore.Mvc;
using Project_1.Data;
using Project_1.Models;
namespace Project_1.Controllers
{
    public class PlanController : Controller
    {
        private readonly AppDbContext _db;
        public PlanController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Plan> names = _db.Plans.ToList();
            return View(names);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Plan plan)
        {
            if (ModelState.IsValid)
            {
                _db.Plans.Add(plan);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var plan = _db.Plans.Find(Id);
            if (plan == null)
            {
                return NotFound();
            }
            return View(plan);
        }

        [HttpPost]
        public IActionResult Edit(Plan plan)
        {
            if (ModelState.IsValid)
            {
                _db.Plans.Update(plan);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var plan = _db.Plans.Find(Id);
            if (plan == null)
            {
                return NotFound();
            }
            return View(plan);
        }

        [HttpPost]
        public IActionResult Delete(Plan plan)
        {
            _db.Plans.Remove(plan);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
