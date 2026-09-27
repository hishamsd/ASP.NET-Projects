namespace Project_1.Dtos.GymDtos
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        //public string Email { get; set; } = string.Empty;
    }

    public class CreateMemberDto
    {
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        //public string Email { get; set; } = string.Empty;
    }
}
