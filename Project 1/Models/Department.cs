using System.ComponentModel.DataAnnotations;

namespace Project_1.Models
{
    public class Department
    {

        [Key]
        public int Id { get; set; }

        public string UID { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }


        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

    }
}
