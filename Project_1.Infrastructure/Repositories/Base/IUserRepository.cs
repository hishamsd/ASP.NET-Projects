using Project_1.Domain.Models;

namespace Project_1.Infrastructure.Repositories.Base
{
    public interface IUserRepository : IRepository<User>
    {
        User? GetByUsername(string username);
        User? GetByEmail(string email);

        User? GetByUid(string uid);

        List<int> GetUserRoleIds(int userId);

        void SetUserRoles(int userId, List<int> roleIds);

        List<UserFile> GetUserFiles(int userId);

        void AddUserFile(UserFile userFile);
    }
}