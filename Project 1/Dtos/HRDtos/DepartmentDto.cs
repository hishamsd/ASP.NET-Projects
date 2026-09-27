namespace Project_1.Dtos.HRDtos
{
    public class DepartmentDto
    {
        public int Id { get; set; }

        public string UID { get; set; }
        public string Name { get; set; }

    }

    public class CreateDepartmentDto
    {
        public string Name { get; set; }
    }
    public class UpdateDepartmentDto : JobDto
    {

    }
    }

