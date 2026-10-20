using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_1.Application.Dtos.HRDtos;
using Project_1.Application.Services.Base;

namespace Project_1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("GetEmployees")]
        [Authorize(Policy = "Read")]
        public IActionResult Emp(string? name = null)
        {
            try
            {
                var employees = _employeeService.GetEmployeesImprove();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Create")]
        public IActionResult CreateEmployee([FromForm] CreateEmployeeDto createEmployeeDto)
        {
            try
            {
                _employeeService.CreateEmployee(createEmployeeDto);
                return Ok("Data Added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
