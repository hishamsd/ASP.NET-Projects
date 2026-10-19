using Project_1.Domain.Models;
using Project_1.Application.Dtos.GymDtos;
using Project_1.Infrastructure.Repositories.Base;
using Project_1.Application.Services.Base;

namespace Project_1.Application.Services
{
    public class SubscribeService : ISubscribeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubscribeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MobileSubscriptionDto>> GetUserSubscriptionsAsync(int userId)
        {
            var member = await _unitOfWork.MemberRepo.GetByUserIdAsync(userId);

            if (member == null)
            {
                return new List<MobileSubscriptionDto>();
            }

            var subscriptions = await _unitOfWork.SubscribeRepo.GetByMemberIdAsync(member.Id);
            var now = DateTime.Now;

            return subscriptions.Select(subscription => new MobileSubscriptionDto
            {
                Id = subscription.Id,
                MemberId = subscription.MemberId,
                PlanId = subscription.PlanId,
                PlanName = subscription.Plan.Name,
                CategoryName = subscription.Plan.Category?.Name ?? "",
                EmployeeId = subscription.EmployeeId,
                EmployeeName = subscription.Employee.Name,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                IsActive = subscription.StartDate <= now && subscription.EndDate >= now
            }).ToList();
        }

        public IEnumerable<Subscribe> GetAll()
        {
            return _unitOfWork.SubscribeRepo.GetAllWithDetails();
        }

        public Subscribe? GetById(int id)
        {
            return _unitOfWork.SubscribeRepo.GetById(id);
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _unitOfWork.MemberRepo.GetAll();
        }

        public IEnumerable<Plan> GetAllPlans()
        {
            return _unitOfWork.PlanRepo.GetAll();
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _unitOfWork.EmployeeRepo.GetAll();
        }

        public void Create(Subscribe subscription)
        {
            var newSubscription = new Subscribe
            {
                MemberId = subscription.MemberId,
                PlanId = subscription.PlanId,
                EmployeeId = subscription.EmployeeId,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate
            };

            _unitOfWork.SubscribeRepo.Add(newSubscription);
            _unitOfWork.Save();
        }

        public bool Update(Subscribe subscription)
        {
            var existingSubscription =
                _unitOfWork.SubscribeRepo.GetById(subscription.Id);

            if (existingSubscription == null)
            {
                return false;
            }

            existingSubscription.MemberId = subscription.MemberId;
            existingSubscription.PlanId = subscription.PlanId;
            existingSubscription.EmployeeId = subscription.EmployeeId;
            existingSubscription.StartDate = subscription.StartDate;
            existingSubscription.EndDate = subscription.EndDate;

            _unitOfWork.Save();

            return true;
        }

        public bool Delete(int id)
        {
            var subscription =
                _unitOfWork.SubscribeRepo.GetById(id);

            if (subscription == null)
            {
                return false;
            }

            _unitOfWork.SubscribeRepo.Delete(subscription);
            _unitOfWork.Save();

            return true;
        }
    }
}