using Microsoft.AspNetCore.Mvc;
using Project_1.Application.Services.Base;

namespace Project_1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var categories = _categoryService.GetCategories();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormFile? image)
        {
            var category = _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            if (image == null)
            {
                ModelState.AddModelError("image", "اختر صورة للفئة.");
            }

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            try
            {
                if (!_categoryService.UpdateImage(id, image!))
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("image", ex.Message);
                return View(category);
            }
        }
    }
}
