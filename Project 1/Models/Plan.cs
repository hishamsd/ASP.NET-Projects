using System.ComponentModel.DataAnnotations.Schema;

namespace Project_1.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }

        public ICollection<Member>? Members { get; set; } = new List<Member>();
    }
}


