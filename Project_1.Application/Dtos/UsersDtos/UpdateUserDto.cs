using System.ComponentModel.DataAnnotations.Schema;
namespace Project_1.Application.Dtos.UsersDtos

{
  
        public class UpdateUserDto : CreateUserDto
        {
            public int Id { get; set; }



        }
}
