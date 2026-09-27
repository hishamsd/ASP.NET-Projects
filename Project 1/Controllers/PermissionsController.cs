using Microsoft.AspNetCore.Mvc;
using Project_1.Data;
using Project_1.Models;
namespace Project_1.Controllers
{
    public class PermissionsController : Controller
    {
        private readonly AppDbContext _db;

        public PermissionsController(AppDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            IEnumerable<Permission> permissions = _db.Permissions.ToList();
            return View(permissions);
        }



        [HttpPost]
        public IActionResult Create(Permission permission)
        {
            if (ModelState.IsValid)
            {
                _db.Permissions.Add(permission);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(permission);
        }



        [HttpPost]
        public IActionResult Edit(Permission permission)
        {
            if (ModelState.IsValid)
            {
                _db.Permissions.Update(permission);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(permission);
        }



        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var permission = _db.Permissions.Find(id);
            if (permission == null)
            {
                return NotFound();
            }
            _db.Permissions.Remove(permission);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
