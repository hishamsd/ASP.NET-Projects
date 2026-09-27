namespace Project_1.Dtos
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string UID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class CreatePermissionDto
    {
        public string Name { get; set; } = string.Empty;
    }
    public class UpdatePermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
