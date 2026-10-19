using Microsoft.EntityFrameworkCore;
using Project_1.Infrastructure.Data;
using Project_1.Domain.Models;
using Project_1.Infrastructure.Repositories.Base;

namespace Project_1.Infrastructure.Repositories
{
    public class PlanRepository : Repository<Plan>, IPlanRepository
    {
        private readonly AppDbContext _db;

        public async Task<List<Plan>> GetByCategoryAsync(int? categoryId)
        {
            var plans = _db.Plans
                .AsNoTracking()
                .Include(plan => plan.Category)
                .Where(plan => plan.CategoryId != null);

            if (categoryId.HasValue)
            {
                plans = plans.Where(plan => plan.CategoryId == categoryId.Value);
            }

            return await plans
                .OrderBy(plan => plan.DurationMonths)
                .ThenBy(plan => plan.Id)
                .ToListAsync();
        }

        public PlanRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}