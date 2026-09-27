
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project_1.Models;

namespace Project_1.Dtos.HRDtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string UID { get; set; }
        public string Name { get; set; }
        public string? ImageURL { get; set; }

        public string JobName { get; set; }

        public string DepartmentName { get; set; }

    }

    public class CreateEmployeeDto
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int JobId { get; set; }
        public int DepartmentId { get; set; }

    }

    public class UpdateEmployeeDto : CreateDepartmentDto
    {
        public int Id { set; get; }

        public string UID { set; get; }
    }
}
