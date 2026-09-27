using Project_1.Models;
using Project_1.Repositories.Base;

namespace Project_1.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User? GetByUsername(string username);
    }
}
