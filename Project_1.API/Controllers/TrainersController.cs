using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_1.Application.Services.Base;
namespace Project_1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainersController : ControllerBase
    {
        private readonly IMobileCatalogService _service;
        public TrainersController(IMobileCatalogService service) { _service = service; }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetTrainers() => Ok(_service.GetTrainers());
    }
}
