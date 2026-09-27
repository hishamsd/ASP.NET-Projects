using Project_1.Dtos.HRDtos;
using Project_1.Models;
using Project_1.Repositories.Base;

namespace Project_1.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        IEnumerable<Employee> GetEmployeesWithJobAndDept();
        IEnumerable<EmployeeDto> GetEmployeesImprove();
    }
}
