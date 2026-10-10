using Project_1.Infrastructure.Data;
using Project_1.Domain.Models;
using Project_1.Infrastructure.Repositories.Base;

namespace Project_1.Infrastructure.Repositories
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
