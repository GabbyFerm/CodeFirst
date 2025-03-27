using Bogus;
using CodeFirst.Models;  

namespace CodeFirst.Database
{
    public class DatabaseSeeder
    {
        private readonly CodeFirstDB _context;

        public DatabaseSeeder(CodeFirstDB context)
        {
            _context = context;
        }

        public async Task SeedAsync() 
        {
            if (_context.Users.Any() && _context.Roles.Any() && _context.Addresses.Any())
                return;

            // Seed roles
            var roleFaker = new Faker<Role>()
                .RuleFor(role => role.Name, fake => fake.PickRandom("Admin", "User"));

            var roles = roleFaker.Generate(2); // Generate 2 roles, admin and user
            await _context.Roles.AddRangeAsync(roles);

            await _context.SaveChangesAsync();

            // Seed users
            var userFaker = new Faker<User>()
            .RuleFor(user => user.Name, fake => fake.Name.FullName())
            .RuleFor(user => user.Email, fake => fake.Internet.Email())
            .RuleFor(user => user.Password, fake => fake.Internet.Password())
            .RuleFor(user => user.PhoneNumber, fake => fake.Phone.PhoneNumber())
            .RuleFor(user => user.DateTime, fake => fake.Date.Past())
            .RuleFor(user => user.RoleId, fake => fake.PickRandom(roles).Id); // Link users to a role

            var users = userFaker.Generate(10);
            await _context.Users.AddRangeAsync(users);

            await _context.SaveChangesAsync();

            // Seed Addresses & assign to users
            var addressFaker = new Faker<Address>()
            .RuleFor(address => address.Street, fake => fake.Address.StreetAddress())
            .RuleFor(address => address.City, fake => fake.Address.City())
            .RuleFor(address => address.Country, fake => fake.Address.Country())
            .RuleFor(address => address.PostalCode, fake => fake.Address.ZipCode());

            var addresses = addressFaker.Generate(10);

            // Assign a random address to each user
            for(int index = 0; index < users.Count; index++)
{
                if (index < addresses.Count)
                {
                    users[index].Address = addresses[index];
                }
            }

            await _context.Addresses.AddRangeAsync(addresses);

            await _context.SaveChangesAsync();
        }
    }
}
