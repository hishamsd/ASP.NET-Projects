using Microsoft.EntityFrameworkCore;
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
        public Role? GetByIdWithPermissions(int id)
        {
            return _db.Roles
                .Include(role => role.Permissions)
                .FirstOrDefault(role => role.Id == id);
        }
    }
}
