using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_1.Domain.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public string UID { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(1, 120)]
        public int DurationMonths { get; set; } = 1;
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? Description { get; set; }

        public ICollection<Subscribe>? Subscriptions { get; set; }
    }
}


