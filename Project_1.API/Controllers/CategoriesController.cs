using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_1.Application.Services.Base;
namespace Project_1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMobileCatalogService _service;
        public CategoriesController(IMobileCatalogService service) { _service = service; }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetCategories() => Ok(_service.GetCategories());
    }
}
