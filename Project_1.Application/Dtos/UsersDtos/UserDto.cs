using System.ComponentModel.DataAnnotations.Schema;
namespace Project_1.Application.Dtos.UsersDtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string UID { get; set; }
        public string Name { get; set; }

        public string? ImageURL { get; set; }
    }
}
