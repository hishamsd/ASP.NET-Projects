using Project_1.Data;
using Project_1.Models;
using Project_1.Repositories.Base;

namespace Project_1.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public User? GetByUsername(string username)
        {
            return _db.Users.FirstOrDefault(u => u.Username == username);
        }
    }
}
