using Microsoft.AspNetCore.Mvc;
using Project_1.Dtos.HRDtos;
using Project_1.Repositories;

namespace Project_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _repo;

        public DepartmentController(IDepartmentRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _repo.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var dept = _repo.GetById(id);
            if (dept == null) return NotFound("Department not found");
            return Ok(dept);
        }
    }
}
