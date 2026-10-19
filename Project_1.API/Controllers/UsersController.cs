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

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            bool userFound = _userService.Login(loginDto);

            if (userFound)
            {
                var session = _userService.GetSession(loginDto.Username)!;
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, session.UserId.ToString()),
                    new Claim(ClaimTypes.Name, session.Name)
                };

                foreach (var role in session.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                foreach (var permission in session.Permissions)
                {
                    claims.Add(new Claim("Permission", permission));
                }

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);

                return Ok(new
                {
                    message = "Login Successful",
                    userId = session.UserId
                });
            }
            else
            {
                return Unauthorized(new
                {
                    message = "Invalid username or password"
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            if (string.IsNullOrWhiteSpace(registerDto.Name) ||
                string.IsNullOrWhiteSpace(registerDto.Email) ||
                string.IsNullOrWhiteSpace(registerDto.Username) ||
                string.IsNullOrWhiteSpace(registerDto.Password))
            {
                return BadRequest(new
                {
                    message = "All fields are required"
                });
            }

            if (registerDto.Password.Length < 6)
            {
                return BadRequest(new
                {
                    message = "Password must be at least 6 characters"
                });
            }

            bool isRegistered = _userService.Register(registerDto);

            if (isRegistered)
            {
                return Ok(new
                {
                    message = "Registration Successful"
                });
            }

            return Conflict(new
            {
                message = "Username or Email already exists"
            });
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var profile = _userService.GetProfile(userId);

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [Authorize]
        [HttpPut("profile")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            bool isUpdated = _userService.UpdateProfile(userId, dto);

            if (!isUpdated)
            {
                return NotFound();
            }

            return Ok(new { message = "Profile Updated" });
        }
    }
}
