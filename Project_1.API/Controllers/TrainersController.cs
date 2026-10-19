using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_1.Application.Services.Base;

namespace Project_1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainersController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public TrainersController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetTrainers()
        {
            var trainers = await _employeeService.GetTrainersAsync();

            return Ok(trainers);
        }
    }
}
