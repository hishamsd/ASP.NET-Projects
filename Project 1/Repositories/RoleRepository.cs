using Project_1.Data;
using Project_1.Models;
using Project_1.Repositories.Base;

namespace Project_1.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly AppDbContext _db;

        public RoleRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
