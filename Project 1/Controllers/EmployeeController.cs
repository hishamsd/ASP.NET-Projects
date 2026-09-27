using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Project_1.Data;
using Project_1.Models;

namespace Project_1.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _db;

        public EmployeeController(AppDbContext db)
        {
            _db = db;
        }

      
        public IActionResult Index()
        {
            var employees = _db.Employees.ToList();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Add(employee);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

       
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Update(employee);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // حذف موظف المباشر مع Popup
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var employee = _db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            _db.Employees.Remove(employee);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
