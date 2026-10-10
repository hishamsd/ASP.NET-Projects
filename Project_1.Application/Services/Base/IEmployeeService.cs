using Project_1.Dtos.HRDtos;
using Project_1.Domain.Models;

namespace Project_1.Services.Base
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDto> GetEmployeesImprove();

        IEnumerable<EmployeeDto> GetEmployeesWithJobAndDept();

        IEnumerable<Department> GetAllDepts();

        IEnumerable<Job> GetAllJobs();

        void CreateEmployee(CreateEmployeeDto employeeDto);
    

    Employee? GetEmployeeById(int id);

        bool UpdateEmployee(int id, CreateEmployeeDto employeeDto);

        bool DeleteEmployee(int id);
    }
}
