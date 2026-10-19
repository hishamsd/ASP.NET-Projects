using System.ComponentModel.DataAnnotations;
namespace Project_1.Application.Dtos.UsersDtos
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
    }
}
