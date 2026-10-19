using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Project_1.Application.Dtos.CategoryDtos;
using Project_1.Application.Services.Base;
using Project_1.Infrastructure.Repositories.Base;

namespace Project_1.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public CategoryService(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        public CategoryDto? GetById(int id)
        {
            var category = _unitOfWork.CategoryRepo.GetById(id);

            if (category == null)
            {
                return null;
            }

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                HasPool = category.HasPool,
                ImageURL = category.ImageURL
            };
        }

        public bool UpdateImage(int id, IFormFile image)
        {
            var category = _unitOfWork.CategoryRepo.GetById(id);

            if (category == null)
            {
                return false;
            }

            category.ImageURL = UploadImage(image);
            _unitOfWork.Save();

            return true;
        }

        private string UploadImage(IFormFile image)
        {
            if (image.Length == 0 || image.Length > 5 * 1024 * 1024)
            {
                throw new ArgumentException("اختر صورة بحجم أكبر من صفر ولا يتجاوز 5 ميجابايت.");
            }

            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (extension != ".jpg" && extension != ".jpeg" &&
                extension != ".png" && extension != ".webp")
            {
                throw new ArgumentException("صيغة الصورة يجب أن تكون JPG أو PNG أو WEBP.");
            }

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var webRoot = _environment.WebRootPath
                ?? Path.Combine(_environment.ContentRootPath, "wwwroot");

            var folderPath = Path.Combine(webRoot, "images", "Categories");
            Directory.CreateDirectory(folderPath);

            using (var stream = new FileStream(Path.Combine(folderPath, fileName), FileMode.CreateNew))
            {
                image.CopyTo(stream);
            }

            return $"/images/Categories/{fileName}";
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            var categories = _unitOfWork.CategoryRepo.GetAll();

            return categories.Select(category => new CategoryDto
            {
                Id = category.Id,
                ImageURL = category.ImageURL,
                Name = category.Name,
                Description = category.Description,
                HasPool = category.HasPool
            }).ToList();
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _unitOfWork.CategoryRepo.GetAllAsync();

            return categories.Select(category => new CategoryDto
            {
                Id = category.Id,
                ImageURL = category.ImageURL,
                Name = category.Name,
                Description = category.Description,
                HasPool = category.HasPool
            }).ToList();
        }
    }
}
