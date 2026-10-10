using Project_1.Infrastructure.Data;
using Project_1.Domain.Models;
using Project_1.Infrastructure.Repositories.Base;

namespace Project_1.Infrastructure.Repositories
{
    public class PlanRepository : Repository<Plan>, IPlanRepository
    {
        private readonly AppDbContext _db;

        public PlanRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}