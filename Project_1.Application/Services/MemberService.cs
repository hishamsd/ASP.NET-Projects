using Project_1.Dtos.GymDtos;
using Project_1.Domain.Models;
using Project_1.Repositories.Base;
using Project_1.Services.Base;

namespace Project_1.Services
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<MemberDto> GetAll()
        {
            return _unitOfWork.MemberRepo.GetAll()
                .Select(member => new MemberDto
                {
                    Id = member.Id,
                    Name = member.Name,
                    PhoneNumber = member.PhoneNumber
                })
                .ToList();
        }

        public Member? GetById(int id)
        {
            return _unitOfWork.MemberRepo.GetById(id);
        }

        public IEnumerable<Plan> GetAllPlans()
        {
            return _unitOfWork.PlanRepo.GetAll();
        }

        public void Create(Member member)
        {
            var newMember = new Member
            {
                Name = member.Name,
                PhoneNumber = member.PhoneNumber,
                PlanId = member.PlanId
            };

            _unitOfWork.MemberRepo.Add(newMember);
            _unitOfWork.Save();
        }

        public bool Update(Member member)
        {
            var existingMember =
                _unitOfWork.MemberRepo.GetById(member.Id);

            if (existingMember == null)
            {
                return false;
            }

            existingMember.Name = member.Name;
            existingMember.PhoneNumber = member.PhoneNumber;

            _unitOfWork.Save();

            return true;
        }

        public bool Delete(int id)
        {
            var member = _unitOfWork.MemberRepo.GetById(id);

            if (member == null)
            {
                return false;
            }

            _unitOfWork.MemberRepo.Delete(member);
            _unitOfWork.Save();

            return true;
        }
    }
}