using System.ComponentModel.DataAnnotations.Schema;

namespace Project_1.Models
{
    public class UserFile
    {
        public int Id { get; set; }

        public string UID { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string FileURL { get; set; } = "";

        [ForeignKey(nameof(Users))]
        public int UserId { get; set; }

        public User? Users { get; set; }

    }
}
