using Project_1.Domain.Models;

namespace Project_1.Services.Base
{
    public interface ISubscribeService
    {
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