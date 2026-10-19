using Project_1.Application.Dtos.CategoryDtos;

namespace Project_1.Application.Services.Base
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDto> GetCategories();

        Task<List<CategoryDto>> GetCategoriesAsync();
    }
}
