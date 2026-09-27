using System.ComponentModel.DataAnnotations.Schema;
namespace Project_1.Dtos.UsersDtos
{
    public class UserDto
    {
        public int Id { get; set; }

        public string UID { get; set; }
        public string Name { get; set; }

        public string? ImageURL { get; set; }
    }
}
