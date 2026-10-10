using Microsoft.EntityFrameworkCore;
using Project_1.Data;
using Project_1.Models;
using Project_1.Repositories.Base;

namespace Project_1.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly AppDbContext _db;

        public EmployeeRepository(AppDbContext db) : base(db)
        {
            _db = db;
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