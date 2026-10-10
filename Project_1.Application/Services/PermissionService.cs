using Project_1.Application.Dtos;
using Project_1.Domain.Models;
using Project_1.Infrastructure.Repositories.Base;
using Project_1.Application.Services.Base;

namespace Project_1.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Permission> GetAll()
        {
            return _unitOfWork.PermissionRepo.GetAll();
        }

        public void Create(CreatePermissionDto permissionDto)
        {
            var permission = new Permission
            {
                Name = permissionDto.Name.Trim()
            };

            _unitOfWork.PermissionRepo.Add(permission);
            _unitOfWork.Save();
        }

        public bool Update(UpdatePermissionDto permissionDto)
        {
            var permission =
                _unitOfWork.PermissionRepo.GetById(permissionDto.Id);

            if (permission == null)
            {
                return false;
            }

            permission.Name = permissionDto.Name.Trim();

            _unitOfWork.Save();

            return true;
        }

        public bool Delete(int id)
        {
            var permission =
                _unitOfWork.PermissionRepo.GetById(id);

            if (permission == null)
            {
                return false;
            }

            _unitOfWork.PermissionRepo.Delete(permission);
            _unitOfWork.Save();

            return true;
        }
    }
}