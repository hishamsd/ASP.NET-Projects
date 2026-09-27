using System.ComponentModel.DataAnnotations.Schema;
namespace Project_1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UID { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public string? HashPassword { get; set; }

        public string Username { get; set; }

        public string? ImageURL { get; set; }
        public Employee? Employee { get; set; }

        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
