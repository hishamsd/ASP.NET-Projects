using Microsoft.AspNetCore.Http;
using Project_1.Application.Dtos;
using Project_1.Application.Dtos.UsersDtos;
using Project_1.Domain.Models;

namespace Project_1.Application.Services.Base
{
    public interface IUserService
    {
        bool Login(LoginDto loginDto);
        bool Register(RegisterDto registerDto);
        UserSessionDto? GetSession(string username);
        ProfileDto? GetProfile(int userId);
        bool UpdateProfile(int userId, UpdateProfileDto dto);
        IEnumerable<UserDto> GetAll();

        User? GetById(int id);

        UpdateUserDto? GetForEdit(string uid);

        void Create(CreateUserDto userDto);

        bool Update(UpdateUserDto userDto);

        bool Delete(int id);

        UserRolesVM? GetUserRoles(int userId);

        bool SaveUserRoles(int userId, List<int> roleIds);

        List<UserFile> GetUserFiles(int userId);

        bool AddUserFile(
            int userId,
            string name,
            IFormFile file);
    }
}
