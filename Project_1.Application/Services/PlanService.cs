using Project_1.Domain.Models;
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
            var newPlan = new Plan
            {
                Name = plan.Name,
                Price = plan.Price,
                Description = plan.Description
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

            existingPlan.Name = plan.Name;
            existingPlan.Price = plan.Price;
            existingPlan.Description = plan.Description;

            _unitOfWork.Save();

            return true;
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