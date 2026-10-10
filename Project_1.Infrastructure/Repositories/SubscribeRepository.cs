using Microsoft.EntityFrameworkCore;
using Project_1.Infrastructure.Data;
using Project_1.Domain.Models;
using Project_1.Infrastructure.Repositories.Base;

namespace Project_1.Infrastructure.Repositories
{
    public class SubscribeRepository
        : Repository<Subscribe>, ISubscribeRepository
    {
        private readonly AppDbContext _db;

        public SubscribeRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Subscribe> GetAllWithDetails()
        {
            return _db.Subscriptions
                .Include(subscription => subscription.Member)
                .Include(subscription => subscription.Plan)
                .Include(subscription => subscription.Employee)
                .ToList();
        }

        public IEnumerable<Subscribe> GetActiveSubscriptions()
        {
            return _db.Subscriptions
                .Where(subscription =>
                    subscription.EndDate >= DateTime.Now)
                .Include(subscription => subscription.Member)
                .Include(subscription => subscription.Plan)
                .Include(subscription => subscription.Employee)
                .ToList();
        }
    }
}