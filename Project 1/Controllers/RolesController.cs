using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_1.Data;
using Project_1.Models;

namespace Project_1.Controllers
{
    public class RolesController : Controller
    {
        private readonly AppDbContext _db;

        public RolesController(AppDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            IEnumerable<Role> roles = _db.Roles.ToList();
            return View(roles);
        }



        [HttpPost]
        public IActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                _db.Roles.Add(role);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }



        [HttpPost]
        public IActionResult Edit(Role role)
        {
            if (ModelState.IsValid)
            {
                _db.Roles.Update(role);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }



        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var role = _db.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }
            _db.Roles.Remove(role);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult AssignPermissions(int roleId)
        {
            var role = _db.Roles
                .Include(r => r.Permissions)
                .FirstOrDefault(r => r.Id == roleId);

            if (role == null)
            {
                return NotFound();
            }

            var permissions = _db.Permissions.ToList();

            ViewBag.AllPermissions = permissions;

            ViewBag.AssignedPermissions = role.Permissions
                .Select(p => p.Id)
                .ToList();

            return View(role);
        }


        [HttpPost]
        public IActionResult AssignPermissions(int roleId, List<int> permissionIds)
        {
            var role = _db.Roles
                .Include(r => r.Permissions)
                .FirstOrDefault(r => r.Id == roleId);

            if (role == null)
            {
                return NotFound();
            }


          
            role.Permissions.Clear();


            var permissions = _db.Permissions
                .Where(p => permissionIds.Contains(p.Id))
                .ToList();


            
            foreach (var permission in permissions)
            {
                role.Permissions.Add(permission);
            }


            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
