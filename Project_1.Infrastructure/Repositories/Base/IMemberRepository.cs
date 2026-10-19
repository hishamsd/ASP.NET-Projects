using Project_1.Domain.Models;

namespace Project_1.Infrastructure.Repositories.Base
{
    public interface IMemberRepository : IRepository<Member>
    {
        Member? GetByUserId(int userId);

        Task<Member?> GetByUserIdAsync(int userId);
    }
}
