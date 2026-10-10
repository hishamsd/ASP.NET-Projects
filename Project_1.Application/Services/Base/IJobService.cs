using Project_1.Dtos.HRDtos;
using Project_1.Domain.Models;

namespace Project_1.Services.Base
{
    public interface IJobService
    {
        IEnumerable<JobDto> GetAll();

        JobDto? GetById(int id);

        IEnumerable<Department> GetAllDepts();

        void Create(CreateJobDto dto);

        bool Update(int id, CreateJobDto dto);

        bool Delete(int id);
    }
}