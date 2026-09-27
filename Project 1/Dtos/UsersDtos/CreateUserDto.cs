using System.ComponentModel.DataAnnotations.Schema;
using Project_1.Models;
namespace Project_1.Dtos.UsersDtos
{
    public class CreateUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public string? HashPassword { get; set; }

        public string Username { get; set; }

        public IFormFile? image { get; set; }
    }
    }

