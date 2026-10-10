using Project_1.Application.Dtos.HRDtos;
using Project_1.Domain.Models;

namespace Project_1.Application.Services.Base
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