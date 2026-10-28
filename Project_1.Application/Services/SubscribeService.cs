using Project_1.Domain.Models;
using Project_1.Application.Dtos.GymDtos;
using Project_1.Infrastructure.Repositories.Base;
using Project_1.Application.Services.Base;
using Project_1.Infrastructure.Data;

namespace Project_1.Application.Services
{
    public class SubscribeService : ISubscribeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _db;

        public SubscribeService(IUnitOfWork unitOfWork, AppDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
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
                Status = subscription.StartDate > now ? "Upcoming"
                    : subscription.EndDate < now ? "Expired" : "Active",
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

            var member = _unitOfWork.MemberRepo.GetById(newSubscription.MemberId);
            if (member?.UserId != null)
            {
                _db.Notifications.Add(new Notification
                {
                    UserId = member.UserId.Value,
                    Subscription = newSubscription,
                    Title = "Subscription Created",
                    Message = $"Your subscription starts on {newSubscription.StartDate:yyyy-MM-dd} and ends on {newSubscription.EndDate:yyyy-MM-dd}."
                });
            }
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

            if (existingSubscription.MemberId == subscription.MemberId &&
                existingSubscription.PlanId == subscription.PlanId &&
                existingSubscription.EmployeeId == subscription.EmployeeId &&
                existingSubscription.StartDate == subscription.StartDate &&
                existingSubscription.EndDate == subscription.EndDate)
            {
                return true;
            }

            existingSubscription.MemberId = subscription.MemberId;
            existingSubscription.PlanId = subscription.PlanId;
            existingSubscription.EmployeeId = subscription.EmployeeId;
            existingSubscription.StartDate = subscription.StartDate;
            existingSubscription.EndDate = subscription.EndDate;

            var member = _unitOfWork.MemberRepo.GetById(existingSubscription.MemberId);
            if (member?.UserId != null)
            {
                _db.Notifications.Add(new Notification
                {
                    UserId = member.UserId.Value,
                    Subscription = existingSubscription,
                    Title = "Subscription Updated",
                    Message = $"Your subscription has been updated. It starts on {existingSubscription.StartDate:yyyy-MM-dd} and ends on {existingSubscription.EndDate:yyyy-MM-dd}."
                });
            }

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
