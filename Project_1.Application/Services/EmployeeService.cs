using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Project_1.Dtos.HRDtos;
using Project_1.Domain.Models;
using Project_1.Repositories.Base;
using Project_1.Services.Base;

namespace Project_1.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public EmployeeService(
            IUnitOfWork unitOfWork,
            IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        public IEnumerable<EmployeeDto> GetEmployeesImprove()
        {
            return _unitOfWork.EmployeeRepo.GetEmployeesImprove()
                .Select(employee => new EmployeeDto
                {
                    Id = employee.Id,
                    UID = employee.UID,
                    Name = employee.Name,
                    Email = employee.Email,
                    Phone = employee.Phone,
                    ImageURL = employee.ImageURL,
                    JobName = employee.Jobs?.Name ?? "",
                    DepartmentName = employee.Departments?.Name ?? ""
                })
                .ToList();
        }
        public IEnumerable<EmployeeDto> GetEmployeesWithJobAndDept()
        {
            return _unitOfWork.EmployeeRepo.GetEmployeesWithJobAndDept()
                .Select(employee => new EmployeeDto
                {
                    Id = employee.Id,
                    UID = employee.UID,
                    Name = employee.Name,
                    Email = employee.Email,
                    Phone = employee.Phone,
                    ImageURL = employee.ImageURL,
                    JobName = employee.Jobs?.Name ?? "",
                    DepartmentName = employee.Departments?.Name ?? ""
                })
                .ToList();
        }

        public IEnumerable<Department> GetAllDepts()
        {
            return _unitOfWork.DepartmentRepo.GetAll();
        }

        public IEnumerable<Job> GetAllJobs()
        {
            return _unitOfWork.JobRepo.GetAll();
        }

        public void CreateEmployee(CreateEmployeeDto employeeDto)
        {
            var employee = new Employee
            {
                Name = employeeDto.Name,
                Email = employeeDto.Email,
                Phone = employeeDto.Phone,
                JobId = employeeDto.JobId,
                DepartmentId = employeeDto.DepartmentId
            };

            if (employeeDto.Image != null &&
                employeeDto.Image.Length > 0)
            {
                employee.ImageURL = UploadImage(employeeDto.Image);
            }

            _unitOfWork.EmployeeRepo.Add(employee);
            _unitOfWork.Save();
        }
        public Employee? GetEmployeeById(int id)
        {
            return _unitOfWork.EmployeeRepo.GetById(id);
        }

        public bool UpdateEmployee(
            int id,
            CreateEmployeeDto employeeDto)
        {
            var employee = _unitOfWork.EmployeeRepo.GetById(id);

            if (employee == null)
            {
                return false;
            }

            var imageUrl = employee.ImageURL;

            if (employeeDto.Image != null &&
                employeeDto.Image.Length > 0)
            {
                imageUrl = UploadImage(employeeDto.Image);
            }

            employee.Name = employeeDto.Name;
            employee.Email = employeeDto.Email;
            employee.Phone = employeeDto.Phone;
            employee.JobId = employeeDto.JobId;
            employee.DepartmentId = employeeDto.DepartmentId;
            employee.ImageURL = imageUrl;

            _unitOfWork.Save();

            return true;
        }

        public bool DeleteEmployee(int id)
        {
            var employee = _unitOfWork.EmployeeRepo.GetById(id);

            if (employee == null)
            {
                return false;
            }

            _unitOfWork.EmployeeRepo.Delete(employee);
            _unitOfWork.Save();

            return true;
        }
        private string UploadImage(IFormFile image)
        {
            var extension =
                Path.GetExtension(image.FileName).ToLowerInvariant();

            if (extension != ".jpg" &&
                extension != ".jpeg" &&
                extension != ".png" &&
                extension != ".webp")
            {
                throw new ArgumentException(
                    "صيغة الصورة يجب أن تكون JPG أو PNG أو WEBP.");
            }

            var fileName = $"{Guid.NewGuid():N}{extension}";

            var webRoot = _environment.WebRootPath
                ?? Path.Combine(_environment.ContentRootPath, "wwwroot");

            var folderPath =
                Path.Combine(webRoot, "images", "Employees");

            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(
                filePath,
                FileMode.CreateNew))
            {
                image.CopyTo(stream);
            }

            return $"/images/Employees/{fileName}";
        }
    }
}
