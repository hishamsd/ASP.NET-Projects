using Project_1.Dtos.HRDtos;

namespace Project_1.Services.Base
{
    public interface IDepartmentService
    {
        IEnumerable<DepartmentDto> GetAll();

        DepartmentDto? GetById(int id);

        void Create(CreateDepartmentDto dto);

        bool Update(int id, CreateDepartmentDto dto);

        bool Delete(int id);
    }
}