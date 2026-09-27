using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_1.Models
{
    public class Subscribe
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Member")]
        public int MemberId { get; set; }
        public Member Member { get; set; }

        [Required]
        [ForeignKey("Plan")]
        public int PlanId { get; set; }
        public Plan Plan { get; set; }

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
    }
} 
