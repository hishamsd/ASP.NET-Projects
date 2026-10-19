using Microsoft.EntityFrameworkCore;
using Project_1.Infrastructure.Data;
using Project_1.Domain.Models;
using Project_1.Infrastructure.Repositories.Base;

namespace Project_1.Infrastructure.Repositories
{
    public class PlanRepository : Repository<Plan>, IPlanRepository
    {
        private readonly AppDbContext _db;

        public IEnumerable<Plan> GetByCategory(int? categoryId)
        {
            return _db.Plans.AsNoTracking().Include(plan => plan.Category)
                .Where(plan => plan.CategoryId != null && (!categoryId.HasValue || plan.CategoryId == categoryId))
                .OrderBy(plan => plan.DurationMonths).ThenBy(plan => plan.Id).ToList();
        }

        public PlanRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}