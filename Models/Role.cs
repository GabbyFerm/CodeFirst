namespace CodeFirst.Models
{
    public class Role
    {
        public int Id { get; set; } // Autogenerate in DB
        public string Name { get; set; } // Adim, User

        // Navigation property (one-to-may: one role can have multipe Users)
        public List<User> Users { get; set; } = new List<User>();
    }
}
