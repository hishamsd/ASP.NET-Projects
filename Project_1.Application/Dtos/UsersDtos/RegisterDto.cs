using System.ComponentModel.DataAnnotations;
namespace Project_1.Application.Dtos.UsersDtos
{
    public class RegisterDto
    {
        [Required] public string Name { get; set; } = "";
        [Required, EmailAddress] public string Email { get; set; } = "";
        [Required] public string Username { get; set; } = "";
        [Required, MinLength(6)] public string Password { get; set; } = "";
        public string Phone { get; set; } = "";
    }
}
