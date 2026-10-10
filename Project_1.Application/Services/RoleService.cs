using Project_1.Domain.Models;
using Project_1.Infrastructure.Repositories.Base;
using Project_1.Application.Services.Base;

namespace Project_1.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Role> GetAll()
        {
            return _unitOfWork.RoleRepo.GetAll();
        }

        public Role? GetByIdWithPermissions(int id)
        {
            return _unitOfWork.RoleRepo.GetByIdWithPermissions(id);
        }

        public IEnumerable<Permission> GetAllPermissions()
        {
            return _unitOfWork.PermissionRepo.GetAll();
        }

        public void Create(string name)
        {
            var role = new Role
            {
                Name = name.Trim()
            };

            _unitOfWork.RoleRepo.Add(role);
            _unitOfWork.Save();
        }

        public bool Update(int id, string name)
        {
            var role = _unitOfWork.RoleRepo.GetById(id);

            if (role == null)
            {
                return false;
            }

            role.Name = name.Trim();

            _unitOfWork.Save();

            return true;
        }

        public bool Delete(int id)
        {
            var role = _unitOfWork.RoleRepo.GetById(id);

            if (role == null)
            {
                return false;
            }

            _unitOfWork.RoleRepo.Delete(role);
            _unitOfWork.Save();

            return true;
        }

        public bool AssignPermissions(
            int roleId,
            List<int> permissionIds)
        {
            var role = GetByIdWithPermissions(roleId);

            if (role == null)
            {
                return false;
            }

            var permissions = _unitOfWork.PermissionRepo.GetAll()
                .Where(permission =>
                    permissionIds.Contains(permission.Id))
                .ToList();

            role.Permissions.Clear();

            foreach (var permission in permissions)
            {
                role.Permissions.Add(permission);
            }

            _unitOfWork.Save();

            return true;
        }
    }
}