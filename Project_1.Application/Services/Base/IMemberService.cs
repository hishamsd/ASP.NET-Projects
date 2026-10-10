using Project_1.Application.Dtos.GymDtos;
using Project_1.Domain.Models;

namespace Project_1.Application.Services.Base
{
    public interface IMemberService
    {
        IEnumerable<MemberDto> GetAll();

        Member? GetById(int id);

        IEnumerable<Plan> GetAllPlans();

        void Create(Member member);

        bool Update(Member member);

        bool Delete(int id);
    }
}