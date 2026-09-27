using Project_1.Data;
using Project_1.Dtos.GymDtos;
using Project_1.Models;
using Project_1.Repositories.Base;

namespace Project_1.Repositories
{
    public class SubscribeRepository : Repository<Subscribe>, ISubscribeRepository
    {
        private readonly AppDbContext _db;

        public SubscribeRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SubscribeDto> GetActiveSubscriptions()
        {
            return _db.Subscriptions
                .Where(s => s.EndDate >= DateTime.Now)
                .Select(s => new SubscribeDto
                {
                    Id = s.Id,
                    MemberId = s.MemberId,
                    MemberName = s.Member.Name,
                    PlanId = s.PlanId,
                    PlanName = s.Plan.Name,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    IsActive = true
                })
                .ToList();
        }
    }
}