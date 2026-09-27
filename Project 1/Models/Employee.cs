using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_1.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        public string UID { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }


        public string? ImageURL { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }


        [ForeignKey(nameof(Job))]
        public int JobId { get; set; }

        public Job? Jobs { get; set; }


        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }
        public Department? Departments { get; set; }
    }
}