using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_1.Application.Dtos.HRDtos;
using Project_1.Application.Services.Base;

namespace Project_1.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _service;
    public EmployeesController(IEmployeeService service) => _service = service;

    [HttpGet]
    [Authorize(Policy = "Read")]
    public IActionResult GetAll() => Ok(_service.GetEmployeesImprove());

    [HttpGet("{id:int}")]
    [Authorize(Policy = "Read")]
    public IActionResult GetById(int id)
    {
        var item = _service.GetEmployeesImprove().FirstOrDefault(employee => employee.Id == id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [Authorize(Policy = "Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create([FromForm] CreateEmployeeDto dto)
    {
        try { _service.CreateEmployee(dto); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "Edit")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [FromForm] CreateEmployeeDto dto)
    {
        try { return _service.UpdateEmployee(id, dto) ? NoContent() : NotFound(); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id) => _service.DeleteEmployee(id) ? NoContent() : NotFound();
}
