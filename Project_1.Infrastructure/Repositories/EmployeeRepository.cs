using Microsoft.EntityFrameworkCore;
using Project_1.Infrastructure.Data;
using Project_1.Domain.Models;
using Project_1.Infrastructure.Repositories.Base;

namespace Project_1.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly AppDbContext _db;

        public EmployeeRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Employee> GetTrainers()
        {
            return _db.Employees.AsNoTracking().Include(employee => employee.Jobs)
                .Where(employee => employee.Jobs != null && employee.Jobs.IsTrainer).ToList();
        }

        public bool IsTrainer(int employeeId)
        {
            return _db.Employees.Any(employee => employee.Id == employeeId
                && employee.Jobs != null && employee.Jobs.IsTrainer);
        }

        public IEnumerable<Employee> GetEmployeesImprove()
        {
            return _db.Employees
                .AsNoTracking()
                .Include(e => e.Jobs)
                .Include(e => e.Departments)
                .ToList();
        }

        public IEnumerable<Employee> GetEmployeesWithJobAndDept()
        {
            return _db.Employees
                .Include(e => e.Jobs)
                .Include(e => e.Departments)
                .ToList();
        }
    }
}