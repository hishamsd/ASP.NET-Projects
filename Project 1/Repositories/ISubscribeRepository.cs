using Project_1.Dtos.GymDtos;
using Project_1.Models;
using Project_1.Repositories.Base;

namespace Project_1.Repositories
{
    public interface ISubscribeRepository : IRepository<Subscribe>
    {
        IEnumerable<SubscribeDto> GetActiveSubscriptions();
    }
}