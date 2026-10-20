using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_1.Infrastructure.Data;

namespace Project_1.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    public AuthController(AppDbContext db)
    {
        _db = db;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(user => user.Username == request.Username);
        if (user == null || string.IsNullOrWhiteSpace(user.HashPassword) ||
            !BCrypt.Net.BCrypt.Verify(request.Password, user.HashPassword))
            return Unauthorized(new { message = "اسم المستخدم أو كلمة المرور غير صحيحة." });

        var roleIds = await _db.RoleUsers.Where(role => role.UserId == user.Id)
            .Select(role => role.RoleId).ToListAsync();
        var roles = await _db.Roles.Include(role => role.Permissions)
            .Where(role => roleIds.Contains(role.Id)).ToListAsync();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, string.IsNullOrWhiteSpace(user.Name) ? user.Username : user.Name)
        };
        if (!string.IsNullOrWhiteSpace(user.ImageURL)) claims.Add(new("ImageURL", user.ImageURL));
        claims.AddRange(roles.Select(role => role.Name).Where(name => !string.IsNullOrWhiteSpace(name))
            .Distinct().Select(name => new Claim(ClaimTypes.Role, name)));
        var permissions = roles.SelectMany(role => role.Permissions).Select(permission => permission.Name)
            .Where(name => !string.IsNullOrWhiteSpace(name)).Distinct().ToArray();
        claims.AddRange(permissions.Select(permission => new Claim("Permission", permission)));
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
        return Ok(new { user.Id, user.Name, permissions });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }
}

public class LoginRequest
{
    [Required] public string Username { get; set; } = "";
    [Required] public string Password { get; set; } = "";
}
