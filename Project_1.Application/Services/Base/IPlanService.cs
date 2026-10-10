using Project_1.Domain.Models;

namespace Project_1.Application.Services.Base
{
    public interface IPlanService
    {
        IEnumerable<Plan> GetAll();

        Plan? GetById(int id);

        void Create(Plan plan);

        bool Update(Plan plan);

        bool Delete(int id);
    }
}