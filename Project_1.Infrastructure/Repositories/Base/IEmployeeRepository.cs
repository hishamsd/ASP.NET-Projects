using Project_1.Domain.Models;

namespace Project_1.Infrastructure.Repositories.Base
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<List<Employee>> GetTrainersAsync();

        Task<bool> IsTrainerAsync(int employeeId);
        IEnumerable<Employee> GetEmployeesWithJobAndDept();

        IEnumerable<Employee> GetEmployeesImprove();
    }
}