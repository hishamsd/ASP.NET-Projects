using Project_1.Application.Dtos.GymDtos;
namespace Project_1.Application.Services.Base
{
    public interface IMobileCatalogService
    {
        IEnumerable<CategoryDto> GetCategories();
        IEnumerable<MobilePlanDto> GetPlans(int? categoryId);
        MobilePlanDto? GetPlan(int id);
        IEnumerable<TrainerDto> GetTrainers();
        IEnumerable<MobileSubscriptionDto> GetSubscriptions(int userId);
    }
}
