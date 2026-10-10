using Project_1.Domain.Models;

namespace Project_1.Services.Base
{
    public interface IRoleService
    {
        IEnumerable<Role> GetAll();

        Role? GetByIdWithPermissions(int id);

        IEnumerable<Permission> GetAllPermissions();

        void Create(string name);

        bool Update(int id, string name);

        bool Delete(int id);

        bool AssignPermissions(int roleId, List<int> permissionIds);
    }
}