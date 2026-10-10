using Project_1.Models;

namespace Project_1.Repositories.Base
{
    public interface IRoleRepository : IRepository<Role>
    {
        Role? GetByIdWithPermissions(int id);
    }
}
