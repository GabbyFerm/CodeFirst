using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Database
{
    public class CodeFirstDB : DbContext
    {
        public CodeFirstDB() { }

        public CodeFirstDB(DbContextOptions<CodeFirstDB> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationship between Role and User
            modelBuilder.Entity<Role>()
                .HasMany(role => role.Users)
                .WithOne(user => user.Role)
                .HasForeignKey(user => user.RoleId);

            // Configure one-to-one or optional relationship between User and Address
            modelBuilder.Entity<User>()
                .HasOne(user => user.Address)
                .WithOne(address => address.User)
                .HasForeignKey<Address>(address => address.UserId)
                .OnDelete(DeleteBehavior.SetNull); // This will not delete the Address when the User is deleted, instead it will set the UserId to null.
        }
    }
}
