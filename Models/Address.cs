namespace CodeFirst.Models
{
    public class Address
    {
        public int Id { get; set; } // Autogenerate in DB
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public required string PostalCode { get; set; }

        public int? UserId { get; set; }  // Foreign key for User
        public User? User { get; set; } = null!;
    }
}
