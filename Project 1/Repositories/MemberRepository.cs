using Project_1.Data;
using Project_1.Models;
using Project_1.Repositories.Base;

namespace Project_1.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        private readonly AppDbContext _db;

        public MemberRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
