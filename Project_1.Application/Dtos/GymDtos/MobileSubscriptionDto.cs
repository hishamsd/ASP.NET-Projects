namespace Project_1.Application.Dtos.GymDtos
{
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
