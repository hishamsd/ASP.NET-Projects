using System.ComponentModel.DataAnnotations;

namespace Project_1.Domain.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        [ConcurrencyCheck]
        public Guid Version { get; set; } = Guid.NewGuid();
    }
}
