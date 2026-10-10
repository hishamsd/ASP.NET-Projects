using Project_1.Domain.Models;

namespace Project_1.Infrastructure.Repositories.Base
{
    public interface IRoleRepository : IRepository<Role>
    {
        Role? GetByIdWithPermissions(int id);
    }
}
