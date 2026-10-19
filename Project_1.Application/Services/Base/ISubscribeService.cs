using Project_1.Domain.Models;
using Project_1.Application.Dtos.GymDtos;

namespace Project_1.Application.Services.Base
{
    public interface ISubscribeService
    {
        Task<List<MobileSubscriptionDto>> GetUserSubscriptionsAsync(int userId);

        IEnumerable<Subscribe> GetAll();

        Subscribe? GetById(int id);

        IEnumerable<Member> GetAllMembers();

        IEnumerable<Plan> GetAllPlans();

        IEnumerable<Employee> GetAllEmployees();

        void Create(Subscribe subscription);

        bool Update(Subscribe subscription);

        bool Delete(int id);
    }
}