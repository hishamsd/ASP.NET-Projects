namespace Project_1.Dtos
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string UID { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }

    public class CreateRoleDto
    {
        public string RoleName { get; set; } = string.Empty;
    }
    public class UpdateRoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}