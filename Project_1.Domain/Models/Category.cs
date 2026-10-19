namespace Project_1.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? ImageURL { get; set; }

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool HasPool { get; set; }
        public ICollection<Plan> Plans { get; set; } = new List<Plan>();
    }
}
