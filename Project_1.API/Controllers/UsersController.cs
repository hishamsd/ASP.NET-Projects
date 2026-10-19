using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_1.API.Filters;
using Project_1.Application.Dtos.UsersDtos;
using Project_1.Application.Services.Base;
namespace Project_1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(MobileExceptionFilter))]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) { _userService = userService; }
        [AllowAnonymous]
        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!_userService.Login(loginDto))
                return Unauthorized(new { message = "Invalid username or password" });
            var session = _userService.GetSession(loginDto.Username)!;
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, session.UserId.ToString()),
                new(ClaimTypes.Name, session.Name)
            };
            claims.AddRange(session.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
            claims.AddRange(session.Permissions.Select(permission => new Claim("Permission", permission)));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
            return Ok(new { message = "Login Successful", userId = session.UserId });
        }
        [AllowAnonymous]
        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            if (string.IsNullOrWhiteSpace(registerDto.Name) || string.IsNullOrWhiteSpace(registerDto.Email)
                || string.IsNullOrWhiteSpace(registerDto.Username) || string.IsNullOrWhiteSpace(registerDto.Password))
                return BadRequest(new { message = "All fields are required" });
            if (registerDto.Password.Length < 6)
                return BadRequest(new { message = "Password must be at least 6 characters" });
            return _userService.Register(registerDto)
                ? Ok(new { message = "Registration Successful" })
                : Conflict(new { message = "Username or Email already exists" });
        }
        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            var profile = _userService.GetProfile(CurrentUserId);
            return profile == null ? NotFound() : Ok(profile);
        }
        [Authorize]
        [HttpPut("profile")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            return _userService.UpdateProfile(CurrentUserId, dto)
                ? Ok(new { message = "Profile Updated" }) : NotFound();
        }
        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
