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
}
