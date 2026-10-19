namespace Project_1.Application.Dtos.CategoryDtos
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public string? ImageURL { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public bool HasPool { get; set; }
    }
}
