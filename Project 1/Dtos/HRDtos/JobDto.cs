namespace Project_1.Dtos.HRDtos
{
    public class JobDto
    {
        public int Id { get; set; }
        public string UID { get; set; }
        public string Name { get; set; }
    }
    public class CreateJobDto
    {
        public string Name { get; set; }
    }
    public class UpdateJobDto : JobDto
    { 
}
}
