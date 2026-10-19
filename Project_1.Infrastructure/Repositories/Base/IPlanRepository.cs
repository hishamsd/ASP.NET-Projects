using Project_1.Domain.Models;

namespace Project_1.Infrastructure.Repositories.Base
{
    public interface IPlanRepository : IRepository<Plan>
    {
        Task<List<Plan>> GetByCategoryAsync(int? categoryId);
    }
}
