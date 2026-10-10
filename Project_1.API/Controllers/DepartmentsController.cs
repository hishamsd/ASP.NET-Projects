using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_1.Application.Dtos.HRDtos;
using Project_1.Application.Services.Base;

namespace Project_1.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _service;
    public DepartmentsController(IDepartmentService service) => _service = service;

    [HttpGet]
    [Authorize(Policy = "Read")]
    public IActionResult GetAll() => Ok(_service.GetAll());

    [HttpGet("{id:int}")]
    [Authorize(Policy = "Read")]
    public IActionResult GetById(int id)
    {
        var item = _service.GetById(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [Authorize(Policy = "Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateDepartmentDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("اكتب الاسم.");
        try
        {
            _service.Create(dto);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "Edit")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, CreateDepartmentDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("اكتب الاسم.");
        try { return _service.Update(id, dto) ? NoContent() : NotFound(); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        try { return _service.Delete(id) ? NoContent() : NotFound(); }
        catch (InvalidOperationException ex) { return Conflict(ex.Message); }
    }
}
