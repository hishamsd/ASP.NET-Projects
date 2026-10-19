using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_1.Application.Services.Base;

namespace Project_1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        private readonly IPlanService _planService;

        public PlansController(IPlanService planService)
        {
            _planService = planService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPlans([FromQuery] int? categoryId)
        {
            var plans = await _planService.GetPlansAsync(categoryId);

            return Ok(plans);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPlan(int id)
        {
            var plan = await _planService.GetPlanAsync(id);

            if (plan == null)
            {
                return NotFound();
            }

            return Ok(plan);
        }
    }
}
