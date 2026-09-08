using System.ComponentModel.DataAnnotations.Schema;

namespace Project_1.Models
{


    
        public class Member
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }

            [ForeignKey("Plan")]
            public int? PlanId { get; set; }
            public Plan? Plan { get; set; }
        }
    }


