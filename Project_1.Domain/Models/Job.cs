using System.ComponentModel.DataAnnotations;

namespace Project_1.Domain.Models;
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        public string UID { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }
        public int? DepartmentId { get; set; }

        public Department? Department { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

    }
}
