using Project_1.Application.Dtos.CartDtos;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_1.API.Filters;
using Project_1.Application.Services.Base;

namespace Project_1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(MobileExceptionFilter))]
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
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var item = await _cartService.GetCartAsync(userId);

            return Ok(new { item = item });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart([FromBody] AddCartDto dto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _cartService.AddToCartAsync(userId, dto);

            return Ok(new { message = "Added to cart" });
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            bool isRemoved = await _cartService.RemoveFromCartAsync(userId);

            if (!isRemoved)
            {
                return NotFound();
            }

            return Ok(new { message = "Removed from cart" });
        }

        [HttpPost("confirm")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm([FromBody] ConfirmCartDto dto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var subscription = await _cartService.ConfirmAsync(userId, dto);

            return Ok(subscription);
        }
    }
}
