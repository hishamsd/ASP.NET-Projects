using System.ComponentModel.DataAnnotations;
namespace Project_1.Application.Dtos.UsersDtos
{
    public class ProfileDto
    {
        public int UserId { get; set; }
        public int? MemberId { get; set; }
        public string Name { get; set; } = "";
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string? ImageURL { get; set; }
    }
    public class UpdateProfileDto
    {
        [Required] public string Name { get; set; } = "";
        [Required, EmailAddress] public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
    }
    public class UserSessionDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = "";
        public string[] Roles { get; set; } = Array.Empty<string>();
        public string[] Permissions { get; set; } = Array.Empty<string>();
    }
}
