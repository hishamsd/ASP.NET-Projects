using Project_1.Application.Dtos.CategoryDtos;
using Project_1.Application.Services.Base;
using Project_1.Infrastructure.Repositories.Base;

namespace Project_1.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            var categories = _unitOfWork.CategoryRepo.GetAll();

            return categories.Select(category => new CategoryDto
            {
                Id = category.Id,
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
                Name = category.Name,
                Description = category.Description,
                HasPool = category.HasPool
            }).ToList();
        }
    }
}
