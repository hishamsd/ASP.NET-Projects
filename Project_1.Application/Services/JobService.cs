using Project_1.Dtos.HRDtos;
using Project_1.Domain.Models;
using Project_1.Repositories.Base;
using Project_1.Services.Base;

namespace Project_1.Services
{
    public class JobService : IJobService
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<JobDto> GetAll()
        {
            return _unitOfWork.JobRepo.GetAll()
                .Select(job => new JobDto
                {
                    Id = job.Id,
                    UID = job.UID,
                    Name = job.Name,
                    DepartmentId = job.DepartmentId
                })
                .ToList();
        }

        public JobDto? GetById(int id)
        {
            var job = _unitOfWork.JobRepo.GetById(id);

            if (job == null)
            {
                return null;
            }

            return new JobDto
            {
                Id = job.Id,
                UID = job.UID,
                Name = job.Name,
                DepartmentId = job.DepartmentId
            };
        }

        public IEnumerable<Department> GetAllDepts()
        {
            return _unitOfWork.DepartmentRepo.GetAll();
        }

        public void Create(CreateJobDto dto)
        {
            ValidateDepartment(dto.DepartmentId);

            var job = new Job
            {
                Name = dto.Name.Trim(),
                DepartmentId = dto.DepartmentId
            };

            _unitOfWork.JobRepo.Add(job);
            _unitOfWork.Save();
        }

        public bool Update(int id, CreateJobDto dto)
        {
            var job = _unitOfWork.JobRepo.GetById(id);

            if (job == null)
            {
                return false;
            }

            ValidateDepartment(dto.DepartmentId);

            job.Name = dto.Name.Trim();
            job.DepartmentId = dto.DepartmentId;

            _unitOfWork.Save();

            return true;
        }

        public bool Delete(int id)
        {
            var job = _unitOfWork.JobRepo.GetById(id);

            if (job == null)
            {
                return false;
            }

            var hasEmployees = _unitOfWork.EmployeeRepo.GetAll()
                .Any(employee => employee.JobId == id);

            if (hasEmployees)
            {
                throw new InvalidOperationException(
                    "ما تقدر تحذف الوظيفة لأنها مرتبطة بموظفين.");
            }

            _unitOfWork.JobRepo.Delete(job);
            _unitOfWork.Save();

            return true;
        }

        private void ValidateDepartment(int departmentId)
        {
            var department =
                _unitOfWork.DepartmentRepo.GetById(departmentId);

            if (department == null)
            {
                throw new ArgumentException(
                    "اختر قسمًا موجودًا.");
            }
        }
    }
}