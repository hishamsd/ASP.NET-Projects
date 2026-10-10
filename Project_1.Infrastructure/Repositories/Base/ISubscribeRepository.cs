using Project_1.Models;

namespace Project_1.Repositories.Base
{
    public interface ISubscribeRepository : IRepository<Subscribe>
    {
        IEnumerable<Subscribe> GetActiveSubscriptions();

        IEnumerable<Subscribe> GetAllWithDetails();
    }
}