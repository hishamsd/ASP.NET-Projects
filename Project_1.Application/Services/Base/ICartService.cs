using Project_1.Application.Dtos.GymDtos;
namespace Project_1.Application.Services.Base
{
    public interface ICartService
    {
        CartDto? GetCart(int userId);
        void AddToCart(int userId, AddCartDto dto);
        bool RemoveFromCart(int userId);
        MobileSubscriptionDto Confirm(int userId, ConfirmCartDto dto);
    }
}
