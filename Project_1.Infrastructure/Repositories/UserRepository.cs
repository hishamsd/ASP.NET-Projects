using Project_1.Infrastructure.Data;
using Project_1.Domain.Models;
using Project_1.Infrastructure.Repositories.Base;

namespace Project_1.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public User? GetByUsername(string username)
        {
            return _db.Users.FirstOrDefault(
                user => user.Username == username);
        }

        public User? GetByUid(string uid)
        {
            return _db.Users.FirstOrDefault(
                user => user.UID == uid);
        }

        public List<int> GetUserRoleIds(int userId)
        {
            return _db.RoleUsers
                .Where(role => role.UserId == userId)
                .Select(role => role.RoleId)
                .ToList();
        }

        public void SetUserRoles(int userId, List<int> roleIds)
        {
            var selectedIds = roleIds.Distinct().ToList();

            var oldRoles = _db.RoleUsers
                .Where(role => role.UserId == userId)
                .ToList();

            // Remove roles that are no longer selected.
            var removedRoles = oldRoles
                .Where(role => !selectedIds.Contains(role.RoleId))
                .ToList();

            _db.RoleUsers.RemoveRange(removedRoles);

            // Add only newly selected roles.
            var oldRoleIds = oldRoles
                .Select(role => role.RoleId)
                .ToList();

            foreach (var roleId in selectedIds)
            {
                if (!oldRoleIds.Contains(roleId))
                {
                    _db.RoleUsers.Add(new RoleUser
                    {
                        UserId = userId,
                        RoleId = roleId
                    });
                }
            }
        }

        public List<UserFile> GetUserFiles(int userId)
        {
            return _db.UserFiles
                .Where(file => file.UserId == userId)
                .ToList();
        }

        public void AddUserFile(UserFile userFile)
        {
            _db.UserFiles.Add(userFile);
        }
    }
}