using Project_1.Application.Dtos.CartDtos;
using Project_1.Application.Dtos.GymDtos;

namespace Project_1.Application.Services.Base
{
    public interface ICartService
    {
        Task<CartDto?> GetCartAsync(int userId);

        Task AddToCartAsync(int userId, AddCartDto dto);

        Task<bool> RemoveFromCartAsync(int userId);

        Task<MobileSubscriptionDto> ConfirmAsync(int userId, ConfirmCartDto dto);
    }
}
