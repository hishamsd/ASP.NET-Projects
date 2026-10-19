using Microsoft.EntityFrameworkCore;
using Project_1.Application.Dtos.CartDtos;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_1.Application.Services.Base;

namespace Project_1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var item = await _cartService.GetCartAsync(userId);

                return Ok(new { item = item });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart([FromBody] AddCartDto dto)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                await _cartService.AddToCartAsync(userId, dto);

                return Ok(new { message = "Added to cart" });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart()
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                bool isRemoved = await _cartService.RemoveFromCartAsync(userId);

                if (!isRemoved)
                {
                    return NotFound();
                }

                return Ok(new { message = "Removed from cart" });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("confirm")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm([FromBody] ConfirmCartDto dto)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var subscription = await _cartService.ConfirmAsync(userId, dto);

                return Ok(subscription);
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

            var logger = HttpContext.RequestServices.GetRequiredService<ILogger<CartController>>();
            logger.LogError(exception, "Mobile API request failed");

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Unable to complete the request." });
        }
    }
}
