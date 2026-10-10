using Project_1.Domain.Models;

namespace Project_1.Infrastructure.Repositories.Base
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        IEnumerable<Employee> GetEmployeesWithJobAndDept();

        IEnumerable<Employee> GetEmployeesImprove();
    }
}