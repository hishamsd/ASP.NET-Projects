namespace Project_1.Application.Dtos.GymDtos
{
    public class MobilePlanDto
    {
        public int Id { get; set; }

        public string? ImageURL { get; set; }

        public string Name { get; set; } = "";

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int DurationMonths { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = "";
    }
}
