using Project_1.Dtos.HRDtos;
using Project_1.Domain.Models;
using Project_1.Repositories.Base;
using Project_1.Services.Base;

namespace Project_1.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<DepartmentDto> GetAll()
        {
            return _unitOfWork.DepartmentRepo.GetAll()
                .Select(department => new DepartmentDto
                {
                    Id = department.Id,
                    UID = department.UID,
                    Name = department.Name
                })
                .ToList();
        }

        public DepartmentDto? GetById(int id)
        {
            var department =
                _unitOfWork.DepartmentRepo.GetById(id);

            if (department == null)
            {
                return null;
            }

            return new DepartmentDto
            {
                Id = department.Id,
                UID = department.UID,
                Name = department.Name
            };
        }

        public void Create(CreateDepartmentDto dto)
        {
            var department = new Department
            {
                Name = dto.Name.Trim()
            };

            _unitOfWork.DepartmentRepo.Add(department);
            _unitOfWork.Save();
        }

        public bool Update(int id, CreateDepartmentDto dto)
        {
            var department =
                _unitOfWork.DepartmentRepo.GetById(id);

            if (department == null)
            {
                return false;
            }

            department.Name = dto.Name.Trim();

            _unitOfWork.Save();

            return true;
        }

        public bool Delete(int id)
        {
            var department =
                _unitOfWork.DepartmentRepo.GetById(id);

            if (department == null)
            {
                return false;
            }

            var hasEmployees = _unitOfWork.EmployeeRepo.GetAll()
                .Any(employee => employee.DepartmentId == id);

            var hasJobs = _unitOfWork.JobRepo.GetAll()
                .Any(job => job.DepartmentId == id);

            if (hasEmployees || hasJobs)
            {
                throw new InvalidOperationException(
                    "ما تقدر تحذف القسم لأنه مرتبط بموظفين أو وظائف.");
            }

            _unitOfWork.DepartmentRepo.Delete(department);
            _unitOfWork.Save();

            return true;
        }
    }
}