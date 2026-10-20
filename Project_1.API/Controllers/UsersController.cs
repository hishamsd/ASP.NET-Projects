using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Project_1.Application.Dtos.UsersDtos;
using Project_1.Application.Services.Base;

namespace Project_1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            try
            {
                bool userFound = _userService.Login(loginDto);

                if (userFound)
                {
                    var session = _userService.GetSession(loginDto.Username);

                    if (session == null)
                    {
                        return Unauthorized(new { message = "Invalid username or password" });
                    }

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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            try
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("profile")]
        public IActionResult Profile([FromQuery] int userId)
        {
            try
            {
                if (userId < 1)
                {
                    return BadRequest(new { message = "A valid userId is required." });
                }
                var profile = _userService.GetProfile(userId);

                if (profile == null)
                {
                    return NotFound();
                }

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("profile")]
        public IActionResult UpdateProfile([FromQuery] int userId, [FromBody] UpdateProfileDto dto)
        {
            try
            {
                if (userId < 1)
                {
                    return BadRequest(new { message = "A valid userId is required." });
                }
                bool isUpdated = _userService.UpdateProfile(userId, dto);

                if (!isUpdated)
                {
                    return NotFound();
                }

                return Ok(new { message = "Profile Updated" });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        private IActionResult HandleException(Exception exception)
        {
            if (exception is KeyNotFoundException)
            {
                return NotFound(new { message = exception.Message });
            }

            if (exception is ArgumentException)
            {
                return BadRequest(new { message = exception.Message });
            }

            if (exception is InvalidOperationException)
            {
                return Conflict(new { message = exception.Message });
            }

            if (exception is DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "Data changed. Reload before trying again." });
            }

            if (exception is DbUpdateException)
            {
                return Conflict(new { message = "The operation conflicts with existing data. Reload and try again." });
            }

            var logger = HttpContext.RequestServices.GetRequiredService<ILogger<UsersController>>();
            logger.LogError(exception, "Mobile API request failed");

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Unable to complete the request." });
        }
    }
}
