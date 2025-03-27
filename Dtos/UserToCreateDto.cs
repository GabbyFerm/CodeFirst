namespace CodeFirst.Dtos
{
    public class UserToCreateDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string RoleName { get; set; }
    }
}
