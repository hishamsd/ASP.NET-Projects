using Project_1.Data;
using Project_1.Models;
using Project_1.Repositories.Base;

namespace Project_1.Repositories
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