using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_1.Data;
using Project_1.Models;

namespace Project_1.Controllers
{
    //[Authorize]
    public class MemberController : Controller
    {

        private readonly AppDbContext _db;

        public MemberController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var members = _db.Members.ToList();
            return View(members);
        }
        [HttpGet]
        public IActionResult Create()
        {
            SelectListForPlans();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Member member)
        {
            if (ModelState.IsValid)
            {
                _db.Members.Add(member);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        private void SelectListForPlans()
        {
            IEnumerable<Plan> plans = _db.Plans.ToList();
            SelectList planSelectList = new SelectList(plans, "Id", "Name");
            ViewBag.Plans = planSelectList;
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var member = _db.Members.Find(id);
            if (member == null)
            {
                return NotFound();
            }
            ViewBag.Plans = new SelectList(_db.Plans.ToList(), "Id", "Name");
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Member member)
        {
            if (ModelState.IsValid)
            {
                _db.Members.Update(member);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var member = _db.Members.FirstOrDefault(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            _db.Members.Remove(member);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
        
    