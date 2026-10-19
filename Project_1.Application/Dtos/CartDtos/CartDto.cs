namespace Project_1.Application.Dtos.CartDtos
{
    public class CartDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PlanId { get; set; }

        public string PlanName { get; set; } = "";

        public string CategoryName { get; set; } = "";

        public int DurationMonths { get; set; }

        public decimal Price { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; } = "";

        public Guid Version { get; set; }
    }
}
