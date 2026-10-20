using System.ComponentModel.DataAnnotations;
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
        var permissions = roles.SelectMany(role => role.Permissions).Select(permission => permission.Name)
            .Where(name => !string.IsNullOrWhiteSpace(name)).Distinct().ToArray();
        return Ok(new { user.Id, user.Name, permissions });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return NoContent();
    }
}

public class LoginRequest
{
    [Required] public string Username { get; set; } = "";
    [Required] public string Password { get; set; } = "";
}
