using System.ComponentModel.DataAnnotations;

namespace Project_1.Application.Dtos.CartDtos
{
    public class AddCartDto
    {
        [Range(1, int.MaxValue)]
        public int PlanId { get; set; }

        [Range(1, int.MaxValue)]
        public int EmployeeId { get; set; }
    }
}
