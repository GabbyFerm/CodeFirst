namespace CodeFirst.Models
{
    public class User
    {
        public int Id { get; set; } // Autogenerate in DB
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? PhoneNumber { get; set; } // I don't want to set this from the beginning, but user should be able to fill it in later.
        public DateTime DateTime { get; set; } = DateTime.Now; // Autogenerate in DB

        public int RoleId { get; set; } // Foreign key
        public Role Role { get; set; } = null!;
        public Address? Address { get; set; }
    }
}
