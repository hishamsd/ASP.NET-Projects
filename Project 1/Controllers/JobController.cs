using Microsoft.AspNetCore.Mvc;
using Project_1.Repositories;

namespace Project_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobRepository _repo;

        public JobController(IJobRepository repo)
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
            var job = _repo.GetById(id);
            if (job == null) return NotFound("Job not found");
            return Ok(job);
        }
    }
}
