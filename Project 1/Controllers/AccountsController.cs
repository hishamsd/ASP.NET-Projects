using Microsoft.AspNetCore.Mvc;
using Project_1.Data;

namespace Project_1.Controllers
{
    public class AccountsController : Controller
    {
        private readonly AppDbContext _db;

        public AccountsController(AppDbContext db)
        {
            _db = db;
        }


        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            if (user != null /*&& BCrypt.Net.BCrypt.Verify(password, user.HashPassword)*/)
            {
                // Authentication successful
                // You can set up session or authentication cookie here
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Authentication failed
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }
        }
    }
}
