using System.ComponentModel.DataAnnotations;

namespace Project_1.Application.Dtos.UsersDtos
{
    public class UpdateProfileDto
    {
        [Required]
        public string Name { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        public string Phone { get; set; } = "";
    }
}
