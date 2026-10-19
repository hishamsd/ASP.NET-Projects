using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Project_1.Application.Dtos;
using Project_1.Application.Dtos.UsersDtos;
using Project_1.Domain.Models;
using Project_1.Infrastructure.Repositories.Base;
using Project_1.Application.Services.Base;

namespace Project_1.Application.Services
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

        public bool Login(LoginDto loginDto)
        {
            var user = _unitOfWork.UserRepo.GetByUsername(loginDto.Username.Trim());

            if (user == null || string.IsNullOrWhiteSpace(user.HashPassword))
            {
                return false;
            }

            bool userFound = BCrypt.Net.BCrypt.Verify(
                loginDto.Password,
                user.HashPassword);

            return userFound;
        }

        public bool Register(RegisterDto registerDto)
        {
            if (string.IsNullOrWhiteSpace(registerDto.Name) ||
                string.IsNullOrWhiteSpace(registerDto.Email) ||
                string.IsNullOrWhiteSpace(registerDto.Username) ||
                string.IsNullOrWhiteSpace(registerDto.Password) ||
                registerDto.Password.Length < 6)
            {
                throw new ArgumentException("All fields are required; password must be at least 6 characters.");
            }

            var username = registerDto.Username.Trim();
            var email = registerDto.Email.Trim();

            var existingUsername = _unitOfWork.UserRepo.GetByUsername(username);
            var existingEmail = _unitOfWork.UserRepo.GetByEmail(email);

            if (existingUsername != null || existingEmail != null)
            {
                return false;
            }

            var user = new User
            {
                Name = registerDto.Name.Trim(),
                Email = email,
                Username = username,
                Phone = registerDto.Phone.Trim(),
                Password = "",
                HashPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };

            var member = new Member
            {
                Name = user.Name,
                PhoneNumber = user.Phone,
                User = user
            };

            _unitOfWork.UserRepo.Add(user);
            _unitOfWork.MemberRepo.Add(member);
            _unitOfWork.Save();

            return true;
        }

        public UserSessionDto? GetSession(string username)
        {
            var user = _unitOfWork.UserRepo.GetByUsername(username.Trim());

            if (user == null)
            {
                return null;
            }

            var roleIds = _unitOfWork.UserRepo.GetUserRoleIds(user.Id);
            var roles = new List<Role>();

            foreach (var roleId in roleIds)
            {
                var role = _unitOfWork.RoleRepo.GetByIdWithPermissions(roleId);

                if (role != null)
                {
                    roles.Add(role);
                }
            }

            var roleNames = roles
                .Select(role => role.Name)
                .Distinct()
                .ToArray();

            var permissions = roles
                .SelectMany(role => role.Permissions)
                .Select(permission => permission.Name)
                .Distinct()
                .ToArray();

            return new UserSessionDto
            {
                UserId = user.Id,
                Name = user.Name,
                Roles = roleNames,
                Permissions = permissions
            };
        }

        public ProfileDto? GetProfile(int userId)
        {
            var user = _unitOfWork.UserRepo.GetById(userId);

            if (user == null)
            {
                return null;
            }

            var member = _unitOfWork.MemberRepo.GetByUserId(userId);

            return new ProfileDto
            {
                UserId = user.Id,
                MemberId = member?.Id,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                ImageURL = user.ImageURL
            };
        }

        public bool UpdateProfile(int userId, UpdateProfileDto dto)
        {
            var user = _unitOfWork.UserRepo.GetById(userId);

            if (user == null)
            {
                return false;
            }

            var existingEmail = _unitOfWork.UserRepo.GetByEmail(dto.Email.Trim());

            if (existingEmail != null && existingEmail.Id != userId)
            {
                throw new InvalidOperationException("Email already exists.");
            }

            user.Name = dto.Name.Trim();
            user.Email = dto.Email.Trim();
            user.Phone = dto.Phone.Trim();

            var member = _unitOfWork.MemberRepo.GetByUserId(userId);

            if (member != null)
            {
                member.Name = user.Name;
                member.PhoneNumber = user.Phone;
            }

            _unitOfWork.Save();

            return true;
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
                Password = "",
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
                user.Password = "";
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