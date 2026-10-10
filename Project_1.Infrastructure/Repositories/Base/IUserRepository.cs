using Project_1.Models;

namespace Project_1.Repositories.Base
{
    public interface IUserRepository : IRepository<User>
    {
        User? GetByUsername(string username);

        User? GetByUid(string uid);

        List<int> GetUserRoleIds(int userId);

        void SetUserRoles(int userId, List<int> roleIds);

        List<UserFile> GetUserFiles(int userId);

        void AddUserFile(UserFile userFile);
    }
}