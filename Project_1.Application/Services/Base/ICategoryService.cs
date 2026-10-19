using Microsoft.AspNetCore.Http;
using Project_1.Application.Dtos.CategoryDtos;

namespace Project_1.Application.Services.Base
{
    public interface ICategoryService
    {
        CategoryDto? GetById(int id);

        bool UpdateImage(int id, IFormFile image);

        IEnumerable<CategoryDto> GetCategories();

        Task<List<CategoryDto>> GetCategoriesAsync();
    }
}
