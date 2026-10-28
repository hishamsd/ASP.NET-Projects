using Microsoft.AspNetCore.Mvc;
using Project_1.Application.Services.Base;

namespace Project_1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscribeService _subscribeService;

        public SubscriptionsController(ISubscribeService subscribeService)
        {
            _subscribeService = subscribeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSubscriptions([FromQuery] int userId)
        {
            if (userId < 1)
            {
                return BadRequest(new { message = "A valid userId is required." });
            }
            var subscriptions = await _subscribeService.GetUserSubscriptionsAsync(userId);

            return Ok(subscriptions);
        }

        [HttpGet("user/{userId:int}")]
        public Task<IActionResult> GetMySubscriptions(int userId)
        {
            return GetSubscriptions(userId);
        }
    }
}
