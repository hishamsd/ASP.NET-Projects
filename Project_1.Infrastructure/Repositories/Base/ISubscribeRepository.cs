using Project_1.Domain.Models;

namespace Project_1.Infrastructure.Repositories.Base
{
    public interface ISubscribeRepository : IRepository<Subscribe>
    {
        IEnumerable<Subscribe> GetActiveSubscriptions();

        IEnumerable<Subscribe> GetAllWithDetails();
    }
}