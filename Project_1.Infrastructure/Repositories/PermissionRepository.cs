using Project_1.Data;
using Project_1.Models;
using Project_1.Repositories.Base;

namespace Project_1.Repositories
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        private readonly AppDbContext _db;

        public PermissionRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
