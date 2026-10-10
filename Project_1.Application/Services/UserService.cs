using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Project_1.Dtos;
using Project_1.Dtos.UsersDtos;
using Project_1.Domain.Models;
using Project_1.Repositories.Base;
using Project_1.Services.Base;

namespace Project_1.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public UserService(
            IUnitOfWork unitOfWork,
            IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        public IEnumerable<UserDto> GetAll()
        {
            return _unitOfWork.UserRepo.GetAll()
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    UID = user.UID,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    ImageURL = user.ImageURL
                })
                .ToList();
        }

        public User? GetById(int id)
        {
            return _unitOfWork.UserRepo.GetById(id);
        }

        public UpdateUserDto? GetForEdit(string uid)
        {
            var user = _unitOfWork.UserRepo.GetByUid(uid);

            if (user == null)
            {
                return null;
            }

            return new UpdateUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Username = user.Username,
                Password = ""
            };
        }

        public void Create(CreateUserDto userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Phone = userDto.Phone,
                Username = userDto.Username,
                Password = userDto.Password,
                HashPassword =
                    BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            if (userDto.image != null &&
                userDto.image.Length > 0)
            {
                user.ImageURL = UploadFile(userDto.image);
            }

            _unitOfWork.UserRepo.Add(user);
            _unitOfWork.Save();
        }

        public bool Update(UpdateUserDto userDto)
        {
            var user = GetById(userDto.Id);

            if (user == null)
            {
                return false;
            }

            var imageUrl = user.ImageURL;

            if (userDto.image != null &&
                userDto.image.Length > 0)
            {
                imageUrl = UploadFile(userDto.image);
            }

            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.Phone = userDto.Phone;
            user.Username = userDto.Username;
            user.ImageURL = imageUrl;

            if (!string.IsNullOrWhiteSpace(userDto.Password))
            {
                user.Password = userDto.Password;
                user.HashPassword =
                    BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            }

            _unitOfWork.Save();

            return true;
        }

        public bool Delete(int id)
        {
            var user = GetById(id);

            if (user == null)
            {
                return false;
            }

            _unitOfWork.UserRepo.Delete(user);
            _unitOfWork.Save();

            return true;
        }

        public UserRolesVM? GetUserRoles(int userId)
        {
            var user = GetById(userId);

            if (user == null)
            {
                return null;
            }

            var roles = _unitOfWork.RoleRepo.GetAll();

            var userRoleIds =
                _unitOfWork.UserRepo.GetUserRoleIds(userId);

            return new UserRolesVM
            {
                UserId = user.Id,
                UserName = user.Name,
                Roles = roles.Select(role => new RoleCheckVM
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = userRoleIds.Contains(role.Id)
                }).ToList()
            };
        }

        public bool SaveUserRoles(int userId, List<int> roleIds)
        {
            var user = GetById(userId);

            if (user == null)
            {
                return false;
            }

            var selectedIds = roleIds.Distinct().ToList();

            var existingRoleIds = _unitOfWork.RoleRepo.GetAll()
                .Select(role => role.Id)
                .ToList();

            if (selectedIds.Any(id => !existingRoleIds.Contains(id)))
            {
                throw new ArgumentException(
                    "أحد الرولز المختارة غير موجود. حدّث الصفحة وجرب.");
            }

            _unitOfWork.UserRepo.SetUserRoles(
                userId,
                selectedIds);

            _unitOfWork.Save();

            return true;
        }

        public List<UserFile> GetUserFiles(int userId)
        {
            return _unitOfWork.UserRepo.GetUserFiles(userId);
        }

        public bool AddUserFile(
            int userId,
            string name,
            IFormFile file)
        {
            var user = GetById(userId);

            if (user == null)
            {
                return false;
            }

            var userFile = new UserFile
            {
                UserId = userId,
                Name = name,
                FileURL = UploadFile(file)
            };

            _unitOfWork.UserRepo.AddUserFile(userFile);
            _unitOfWork.Save();

            return true;
        }

        private string UploadFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);

            var fileName = $"{Guid.NewGuid():N}{extension}";

            var webRoot = _environment.WebRootPath
                ?? Path.Combine(
                    _environment.ContentRootPath,
                    "wwwroot");

            var folderPath = Path.Combine(
                webRoot,
                "images",
                "Users");

            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(
                filePath,
                FileMode.CreateNew))
            {
                file.CopyTo(stream);
            }

            return $"/images/Users/{fileName}";
        }
    }
}