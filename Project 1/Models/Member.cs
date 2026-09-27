using System.ComponentModel.DataAnnotations.Schema;

namespace Project_1.Models
{


    
        public class Member
        {
         public int Id { get; set; }
        public string UID { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
         public string PhoneNumber { get; set; }
         public int? PlanId { get; set; }
         public Plan? Plan { get; set; }


        public ICollection<Subscribe>? Subscriptions { get; set; }
    }
    }


