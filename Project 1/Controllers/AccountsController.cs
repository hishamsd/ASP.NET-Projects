using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_1.Infrastructure.Data;

namespace Project_1.Controllers
{
    public class AccountsController : Controller
    {
        private readonly AppDbContext _db;

        public AccountsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            string? username,
            string? password,
            string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(
                    "",
                    "اكتب اسم المستخدم وكلمة المرور.");

                return View();
            }

            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null ||
                string.IsNullOrWhiteSpace(user.HashPassword) ||
                !BCrypt.Net.BCrypt.Verify(password, user.HashPassword))
            {
                ModelState.AddModelError(
                    "",
                    "اسم المستخدم أو كلمة المرور غير صحيحة.");

                return View();
            }

            var roleIds = await _db.RoleUsers
                .Where(x => x.UserId == user.Id)
                .Select(x => x.RoleId)
                .ToListAsync();

            var roles = await _db.Roles
                .Include(role => role.Permissions)
                .Where(role => roleIds.Contains(role.Id))
                .ToListAsync();

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    string.IsNullOrWhiteSpace(user.Name)
                        ? user.Username
                        : user.Name)
            };

            if (!string.IsNullOrWhiteSpace(user.ImageURL))
            {
                claims.Add(new Claim("ImageURL", user.ImageURL));
            }

            var roleNames = roles
                .Select(role => role.Name)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct();

            foreach (var roleName in roleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }

            var permissionNames = roles
                .SelectMany(role => role.Permissions)
                .Select(permission => permission.Name)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct();

            foreach (var permissionName in permissionNames)
            {
                claims.Add(new Claim("Permission", permissionName));
            }

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = false
                });

            if (!string.IsNullOrEmpty(returnUrl) &&
                Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return new ContentResult
            {
                StatusCode = StatusCodes.Status403Forbidden,
                ContentType = "text/plain; charset=utf-8",
                Content = "ما عندك صلاحية للوصول إلى هذه الصفحة أو تنفيذ العملية."
            };
        }
    }
}