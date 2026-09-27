using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_1.Data;
using Project_1.Models;

namespace Project_1.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly AppDbContext _db;

        public SubscriptionsController(AppDbContext db)
        {
            _db = db;
        }

        // 1. عرض جميع الاشتراكات مع جلب بيانات العضو والخطة والموظف
        public IActionResult Index()
        {
            var subscriptions = _db.Subscriptions
                .Include(s => s.Member)
                .Include(s => s.Plan)
                .Include(s => s.Employee)
                .ToList();

            return View(subscriptions);
        }

        // 2. صفحة إضافة اشتراك جديد (GET)
        [HttpGet]
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // 3. حفظ الاشتراك الجديد (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Subscribe subscription)
        {
            if (ModelState.IsValid)
            {
                _db.Subscriptions.Add(subscription);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns();
            return View(subscription);
        }

        // 4. حذف اشتراك مباشر مع Modal
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var subscription = _db.Subscriptions.Find(id);
            if (subscription == null)
            {
                return NotFound();
            }

            _db.Subscriptions.Remove(subscription);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // دالة مساعدة لتعبئة القوائم المنسدلة الثلاث (SelectList)
        private void PopulateDropdowns()
        {
            ViewBag.Members = new SelectList(_db.Members.ToList(), "Id", "Name");
            ViewBag.Plans = new SelectList(_db.Plans.ToList(), "Id", "Name");
            ViewBag.Employees = new SelectList(_db.Employees.ToList(), "Id", "Name");
        }
    }
}