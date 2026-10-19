namespace Project_1.Application.Dtos.UsersDtos
{
    public class UserSessionDto
    {
        public int UserId { get; set; }

        public string Name { get; set; } = "";

        public string[] Roles { get; set; } = Array.Empty<string>();

        public string[] Permissions { get; set; } = Array.Empty<string>();
    }
}
