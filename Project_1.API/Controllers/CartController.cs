using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_1.API.Filters;
using Project_1.Application.Dtos.GymDtos;
using Project_1.Application.Services.Base;
namespace Project_1.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(MobileExceptionFilter))]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService) { _cartService = cartService; }
        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        [HttpGet]
        public IActionResult GetCart() => Ok(new { item = _cartService.GetCart(CurrentUserId) });
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart([FromBody] AddCartDto dto)
        {
            _cartService.AddToCart(CurrentUserId, dto);
            return Ok(new { message = "Added to cart" });
        }
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart() => _cartService.RemoveFromCart(CurrentUserId)
            ? Ok(new { message = "Removed from cart" }) : NotFound();
        [HttpPost("confirm")]
        [ValidateAntiForgeryToken]
        public IActionResult Confirm([FromBody] ConfirmCartDto dto) => Ok(_cartService.Confirm(CurrentUserId, dto));
    }
}
