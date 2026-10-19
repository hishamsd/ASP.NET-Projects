using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_1.Application.Services.Base;
namespace Project_1.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IMobileCatalogService _service;
        public SubscriptionsController(IMobileCatalogService service) { _service = service; }
        [HttpGet]
        public IActionResult GetSubscriptions() => Ok(_service.GetSubscriptions(
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)));
    }
}
