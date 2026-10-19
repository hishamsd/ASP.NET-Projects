using System.ComponentModel.DataAnnotations;
namespace Project_1.Application.Dtos.GymDtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool HasPool { get; set; }
    }
    public class MobilePlanDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationMonths { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = "";
    }
    public class TrainerDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = "";
        public string? ImageURL { get; set; }
    }
    public class AddCartDto
    {
        [Range(1, int.MaxValue)] public int PlanId { get; set; }
        [Range(1, int.MaxValue)] public int EmployeeId { get; set; }
    }
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
    public class ConfirmCartDto
    {
        public Guid Version { get; set; }
    }
    public class MobileSubscriptionDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int PlanId { get; set; }
        public string PlanName { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
