using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Project_1.Data;
using Project_1.Dtos.HRDtos;
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

        public IEnumerable<EmployeeDto> GetEmployeesImprove()
        {
            return _db.Employees
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    UID = e.UID,
                    ImageURL = e.ImageURL,
                    JobName = e.Jobs.Name,
                    DepartmentName = e.Departments.Name
                })
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

