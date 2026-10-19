using Project_1.Domain.Models;
using Project_1.Application.Dtos.GymDtos;
using Project_1.Infrastructure.Repositories.Base;
using Project_1.Application.Services.Base;

namespace Project_1.Application.Services
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MobilePlanDto>> GetPlansAsync(int? categoryId)
        {
            var plans = await _unitOfWork.PlanRepo.GetByCategoryAsync(categoryId);

            return plans.Select(plan => new MobilePlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                DurationMonths = plan.DurationMonths,
                CategoryId = plan.CategoryId!.Value,
                CategoryName = plan.Category!.Name
            }).ToList();
        }

        public async Task<MobilePlanDto?> GetPlanAsync(int id)
        {
            var plans = await GetPlansAsync(null);
            var plan = plans.FirstOrDefault(plan => plan.Id == id);

            return plan;
        }

        public IEnumerable<Plan> GetAll()
        {
            return _unitOfWork.PlanRepo.GetAll();
        }

        public Plan? GetById(int id)
        {
            return _unitOfWork.PlanRepo.GetById(id);
        }

        public void Create(Plan plan)
        {
            Validate(plan);
            var newPlan = new Plan
            {
                Name = plan.Name,
                Price = plan.Price,
                Description = plan.Description,
                CategoryId = plan.CategoryId,
                DurationMonths = plan.DurationMonths
            };

            _unitOfWork.PlanRepo.Add(newPlan);
            _unitOfWork.Save();
        }

        public bool Update(Plan plan)
        {
            var existingPlan =
                _unitOfWork.PlanRepo.GetById(plan.Id);

            if (existingPlan == null)
            {
                return false;
            }

            Validate(plan);
            existingPlan.Name = plan.Name;
            existingPlan.Price = plan.Price;
            existingPlan.Description = plan.Description;
            existingPlan.CategoryId = plan.CategoryId;
            existingPlan.DurationMonths = plan.DurationMonths;

            _unitOfWork.Save();

            return true;
        }

        private void Validate(Plan plan)
        {
            if (plan.DurationMonths < 1 || plan.DurationMonths > 120 || plan.Price < 0)
            {
                throw new ArgumentException("Duration must be 1 to 120 months and price cannot be negative.");
            }

            if (plan.CategoryId.HasValue)
            {
                var category = _unitOfWork.CategoryRepo.GetById(plan.CategoryId.Value);

                if (category == null)
                {
                    throw new ArgumentException("Category not found.");
                }
            }
        }

        public bool Delete(int id)
        {
            var plan = _unitOfWork.PlanRepo.GetById(id);

            if (plan == null)
            {
                return false;
            }

            _unitOfWork.PlanRepo.Delete(plan);
            _unitOfWork.Save();

            return true;
        }
    }
}