using Project_1.Application.Dtos;
using Project_1.Domain.Models;

namespace Project_1.Application.Services.Base
{
    public interface IPermissionService
    {
        IEnumerable<Permission> GetAll();

        void Create(CreatePermissionDto permissionDto);

        bool Update(UpdatePermissionDto permissionDto);

        bool Delete(int id);
    }
}
