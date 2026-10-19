using Microsoft.AspNetCore.Http;
using Project_1.Domain.Models;
using Project_1.Application.Dtos.GymDtos;

namespace Project_1.Application.Services.Base
{
    public interface IPlanService
    {
        Task<List<MobilePlanDto>> GetPlansAsync(int? categoryId);

        Task<MobilePlanDto?> GetPlanAsync(int id);

        IEnumerable<Plan> GetAll();

        Plan? GetById(int id);

        void Create(Plan plan, IFormFile? image = null);

        bool Update(Plan plan, IFormFile? image = null);

        bool Delete(int id);
    }
}