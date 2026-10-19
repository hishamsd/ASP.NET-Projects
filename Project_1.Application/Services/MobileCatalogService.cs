using Project_1.Application.Dtos.GymDtos;
using Project_1.Application.Services.Base;
using Project_1.Domain.Models;
using Project_1.Infrastructure.Repositories.Base;
namespace Project_1.Application.Services
{
    public class MobileCatalogService : IMobileCatalogService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MobileCatalogService(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public IEnumerable<CategoryDto> GetCategories() => _unitOfWork.CategoryRepo.GetAll()
            .Select(category => new CategoryDto { Id = category.Id, Name = category.Name,
                Description = category.Description, HasPool = category.HasPool }).ToList();
        public IEnumerable<MobilePlanDto> GetPlans(int? categoryId) =>
            _unitOfWork.PlanRepo.GetByCategory(categoryId).Select(plan => new MobilePlanDto
            {
                Id = plan.Id, Name = plan.Name, Description = plan.Description, Price = plan.Price,
                DurationMonths = plan.DurationMonths, CategoryId = plan.CategoryId!.Value,
                CategoryName = plan.Category!.Name
            }).ToList();
        public MobilePlanDto? GetPlan(int id) => GetPlans(null).FirstOrDefault(plan => plan.Id == id);
        public IEnumerable<TrainerDto> GetTrainers() => _unitOfWork.EmployeeRepo.GetTrainers()
            .Select(employee => new TrainerDto { EmployeeId = employee.Id, Name = employee.Name,
                ImageURL = employee.ImageURL }).ToList();
        public IEnumerable<MobileSubscriptionDto> GetSubscriptions(int userId)
        {
            var member = _unitOfWork.MemberRepo.GetByUserId(userId);
            if (member == null) return Array.Empty<MobileSubscriptionDto>();
            return _unitOfWork.SubscribeRepo.GetByMemberId(member.Id).Select(ToDto).ToList();
        }
        internal static MobileSubscriptionDto ToDto(Subscribe subscription) => new()
        {
            Id = subscription.Id, MemberId = subscription.MemberId, PlanId = subscription.PlanId,
            PlanName = subscription.Plan.Name, CategoryName = subscription.Plan.Category?.Name ?? "",
            EmployeeId = subscription.EmployeeId, EmployeeName = subscription.Employee.Name,
            StartDate = subscription.StartDate, EndDate = subscription.EndDate,
            IsActive = subscription.StartDate <= DateTime.Now && subscription.EndDate >= DateTime.Now
        };
    }
}
