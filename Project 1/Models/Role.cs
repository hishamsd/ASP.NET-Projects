namespace Project_1.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string UID { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
