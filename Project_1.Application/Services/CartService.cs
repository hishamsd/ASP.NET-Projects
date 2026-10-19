using Project_1.Application.Dtos.GymDtos;
using Project_1.Application.Services.Base;
using Project_1.Domain.Models;
using Project_1.Infrastructure.Repositories.Base;
namespace Project_1.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartService(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public CartDto? GetCart(int userId)
        {
            var item = _unitOfWork.CartRepo.GetByUserId(userId);
            if (item == null) return null;
            return new CartDto
            {
                Id = item.Id, UserId = userId, PlanId = item.PlanId, PlanName = item.Plan.Name,
                CategoryName = item.Plan.Category?.Name ?? "", DurationMonths = item.Plan.DurationMonths,
                Price = item.Plan.Price, EmployeeId = item.EmployeeId, EmployeeName = item.Employee.Name,
                Version = item.Version
            };
        }
        public void AddToCart(int userId, AddCartDto dto)
        {
            if (_unitOfWork.MemberRepo.GetByUserId(userId) == null)
                throw new InvalidOperationException("This account is not linked to a gym member.");
            var plan = _unitOfWork.PlanRepo.GetById(dto.PlanId);
            ValidatePlan(plan);
            if (!_unitOfWork.EmployeeRepo.IsTrainer(dto.EmployeeId))
                throw new ArgumentException("Choose an employee whose job is marked as trainer.");
            var item = _unitOfWork.CartRepo.GetByUserId(userId);
            if (item == null)
            {
                item = new CartItem { UserId = userId, PlanId = dto.PlanId, EmployeeId = dto.EmployeeId };
                _unitOfWork.CartRepo.Add(item);
            }
            else
            {
                item.PlanId = dto.PlanId;
                item.EmployeeId = dto.EmployeeId;
                item.Version = Guid.NewGuid();
            }
            _unitOfWork.Save();
        }
        public bool RemoveFromCart(int userId)
        {
            var item = _unitOfWork.CartRepo.GetByUserId(userId);
            if (item == null) return false;
            _unitOfWork.CartRepo.Delete(item);
            _unitOfWork.Save();
            return true;
        }
        public MobileSubscriptionDto Confirm(int userId, ConfirmCartDto dto)
        {
            var item = _unitOfWork.CartRepo.GetByUserId(userId)
                ?? throw new KeyNotFoundException("Cart is empty.");
            if (dto.Version == Guid.Empty || dto.Version != item.Version)
                throw new InvalidOperationException("Cart changed. Reload it before confirming.");
            ValidatePlan(item.Plan);
            if (!_unitOfWork.EmployeeRepo.IsTrainer(item.EmployeeId))
                throw new ArgumentException("The selected trainer is no longer available.");
            var member = _unitOfWork.MemberRepo.GetByUserId(userId)
                ?? throw new InvalidOperationException("This account is not linked to a gym member.");
            var now = DateTime.Now;
            if (_unitOfWork.SubscribeRepo.GetByMemberId(member.Id).Any(subscription => subscription.EndDate >= now))
                throw new InvalidOperationException("An active or upcoming subscription already exists.");
            var subscription = new Subscribe
            {
                MemberId = member.Id, PlanId = item.PlanId, EmployeeId = item.EmployeeId,
                Plan = item.Plan, Employee = item.Employee, StartDate = now,
                EndDate = now.AddMonths(item.Plan.DurationMonths)
            };
            member.PlanId = item.PlanId;
            _unitOfWork.SubscribeRepo.Add(subscription);
            _unitOfWork.CartRepo.Delete(item);
            // One SaveChanges transaction creates the subscription and removes the cart.
            // The cart concurrency token prevents duplicate concurrent confirmation.
            _unitOfWork.Save();
            return MobileCatalogService.ToDto(subscription);
        }
        private static void ValidatePlan(Plan? plan)
        {
            if (plan == null) throw new KeyNotFoundException("Plan not found.");
            if (plan.CategoryId == null || plan.DurationMonths < 1 || plan.DurationMonths > 120 || plan.Price < 0)
                throw new ArgumentException("Plan needs a category, a valid duration and a valid price.");
        }
    }
}
