using Microsoft.AspNetCore.Mvc;
using Project_1.Application.Dtos;
using Project_1.Application.Dtos.UsersDtos;
using Project_1.Domain.Models;
using Project_1.Application.Services.Base;

namespace Project_1.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_userService.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateUserDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateUserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Password))
            {
                ModelState.AddModelError(
                    nameof(userDto.Password),
                    "اكتب كلمة المرور.");
            }

            if (!ModelState.IsValid)
            {
                return View(userDto);
            }

            _userService.Create(userDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _userService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UpdateUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Username = user.Username,
                Password = ""
            };

            return View(userDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UpdateUserDto userDto)
        {
            // A blank password keeps the current password.
            if (string.IsNullOrWhiteSpace(userDto.Password))
            {
                ModelState.Remove(nameof(userDto.Password));
            }

            if (!ModelState.IsValid)
            {
                return View(userDto);
            }

            var updated = _userService.Update(userDto);

            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var deleted = _userService.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ManageRoles(int id)
        {
            var model = _userService.GetUserRoles(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ManageRoles(UserRolesVM model)
        {
            var selectedIds = model.Roles?
                .Where(role => role.IsSelected)
                .Select(role => role.RoleId)
                .ToList() ?? new List<int>();

            try
            {
                var saved = _userService.SaveUserRoles(
                    model.UserId,
                    selectedIds);

                if (!saved)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                var displayModel =
                    _userService.GetUserRoles(model.UserId);

                if (displayModel == null)
                {
                    return NotFound();
                }

                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);

                foreach (var role in displayModel.Roles)
                {
                    role.IsSelected =
                        selectedIds.Contains(role.RoleId);
                }

                return View(displayModel);
            }
        }

        [HttpGet]
        public IActionResult ManageFiles(int UserId)
        {
            var user = _userService.GetById(UserId);

            if (user == null)
            {
                return NotFound();
            }

            LoadFiles(UserId, user.Name);

            return View(new UserFile
            {
                UserId = UserId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ManageFiles(
            UserFile userFile,
            IFormFile? file)
        {
            var user = _userService.GetById(userFile.UserId);

            if (user == null)
            {
                return NotFound();
            }

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError(
                    "file",
                    "اختر الملف.");
            }

            if (string.IsNullOrWhiteSpace(userFile.Name))
            {
                ModelState.AddModelError(
                    nameof(userFile.Name),
                    "اكتب اسم الملف.");
            }

            if (!ModelState.IsValid)
            {
                LoadFiles(userFile.UserId, user.Name);

                return View(userFile);
            }

            var saved = _userService.AddUserFile(
                userFile.UserId,
                userFile.Name,
                file!);

            if (!saved)
            {
                return NotFound();
            }

            return RedirectToAction(
                nameof(ManageFiles),
                new { UserId = userFile.UserId });
        }

        private void LoadFiles(int userId, string userName)
        {
            ViewBag.Files = _userService.GetUserFiles(userId);
            ViewBag.userName = userName;
        }
    }
}