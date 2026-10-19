using Microsoft.EntityFrameworkCore;
using Project_1.Infrastructure.Data;
using Project_1.Domain.Models;
using Project_1.Infrastructure.Repositories.Base;

namespace Project_1.Infrastructure.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        private readonly AppDbContext _db;

        public async Task<Member?> GetByUserIdAsync(int userId)
        {
            return await _db.Members
                .SingleOrDefaultAsync(member => member.UserId == userId);
        }

        public Member? GetByUserId(int userId)
        {
            return _db.Members.SingleOrDefault(member => member.UserId == userId);
        }

        public MemberRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
