using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_1.Application.Services.Base;
namespace Project_1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlansController : ControllerBase
    {
        private readonly IMobileCatalogService _service;
        public PlansController(IMobileCatalogService service) { _service = service; }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetPlans([FromQuery] int? categoryId) => Ok(_service.GetPlans(categoryId));
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public IActionResult GetPlan(int id)
        {
            var plan = _service.GetPlan(id);
            return plan == null ? NotFound() : Ok(plan);
        }
    }
}
