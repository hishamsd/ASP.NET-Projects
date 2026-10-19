using Project_1.Application.Dtos.CartDtos;
using Project_1.Application.Dtos.GymDtos;
using Project_1.Application.Services.Base;
using Project_1.Domain.Models;
using Project_1.Infrastructure.Repositories.Base;

namespace Project_1.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CartDto?> GetCartAsync(int userId)
        {
            var item = await _unitOfWork.CartRepo.GetByUserIdAsync(userId);

            if (item == null)
            {
                return null;
            }

            return new CartDto
            {
                Id = item.Id,
                UserId = item.UserId,
                PlanId = item.PlanId,
                PlanName = item.Plan.Name,
                CategoryName = item.Plan.Category?.Name ?? "",
                DurationMonths = item.Plan.DurationMonths,
                Price = item.Plan.Price,
                EmployeeId = item.EmployeeId,
                EmployeeName = item.Employee.Name,
                Version = item.Version
            };
        }

        public async Task AddToCartAsync(int userId, AddCartDto dto)
        {
            var member = await _unitOfWork.MemberRepo.GetByUserIdAsync(userId);

            if (member == null)
            {
                throw new InvalidOperationException("This account is not linked to a gym member.");
            }

            var plan = await _unitOfWork.PlanRepo.GetByIdAsync(dto.PlanId);
            ValidatePlan(plan);

            bool trainerExists = await _unitOfWork.EmployeeRepo.IsTrainerAsync(dto.EmployeeId);

            if (!trainerExists)
            {
                throw new ArgumentException("Choose an employee whose job is marked as trainer.");
            }

            var item = await _unitOfWork.CartRepo.GetByUserIdAsync(userId);

            if (item == null)
            {
                item = new CartItem
                {
                    UserId = userId,
                    PlanId = dto.PlanId,
                    EmployeeId = dto.EmployeeId
                };

                _unitOfWork.CartRepo.Add(item);
            }
            else
            {
                item.PlanId = dto.PlanId;
                item.EmployeeId = dto.EmployeeId;
                item.Version = Guid.NewGuid();
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> RemoveFromCartAsync(int userId)
        {
            var item = await _unitOfWork.CartRepo.GetByUserIdAsync(userId);

            if (item == null)
            {
                return false;
            }

            _unitOfWork.CartRepo.Delete(item);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<MobileSubscriptionDto> ConfirmAsync(int userId, ConfirmCartDto dto)
        {
            var item = await _unitOfWork.CartRepo.GetByUserIdAsync(userId);

            if (item == null)
            {
                throw new KeyNotFoundException("Cart is empty.");
            }

            if (dto.Version == Guid.Empty || dto.Version != item.Version)
            {
                throw new InvalidOperationException("Cart changed. Reload it before confirming.");
            }

            ValidatePlan(item.Plan);

            bool trainerExists = await _unitOfWork.EmployeeRepo.IsTrainerAsync(item.EmployeeId);

            if (!trainerExists)
            {
                throw new ArgumentException("The selected trainer is no longer available.");
            }

            var member = await _unitOfWork.MemberRepo.GetByUserIdAsync(userId);

            if (member == null)
            {
                throw new InvalidOperationException("This account is not linked to a gym member.");
            }

            var subscriptions = await _unitOfWork.SubscribeRepo.GetByMemberIdAsync(member.Id);
            var now = DateTime.Now;
            bool subscriptionExists = subscriptions.Any(subscription => subscription.EndDate >= now);

            if (subscriptionExists)
            {
                throw new InvalidOperationException("An active or upcoming subscription already exists.");
            }

            var subscription = new Subscribe
            {
                MemberId = member.Id,
                PlanId = item.PlanId,
                EmployeeId = item.EmployeeId,
                Plan = item.Plan,
                Employee = item.Employee,
                StartDate = now,
                EndDate = now.AddMonths(item.Plan.DurationMonths)
            };

            member.PlanId = item.PlanId;
            _unitOfWork.SubscribeRepo.Add(subscription);
            _unitOfWork.CartRepo.Delete(item);

            // Save the subscription and remove the cart in the same transaction.
            await _unitOfWork.SaveAsync();

            return new MobileSubscriptionDto
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
            };
        }

        private static void ValidatePlan(Plan? plan)
        {
            if (plan == null)
            {
                throw new KeyNotFoundException("Plan not found.");
            }

            if (plan.CategoryId == null ||
                plan.DurationMonths < 1 ||
                plan.DurationMonths > 120 ||
                plan.Price < 0)
            {
                throw new ArgumentException("Plan needs a category, a valid duration and a valid price.");
            }
        }
    }
}
