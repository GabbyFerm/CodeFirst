using CodeFirst.Database;
using CodeFirst.Dtos;
using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Services
{
    public class UserService : IUserService
    {
        private readonly CodeFirstDB _context;

        public UserService(CodeFirstDB context) 
        { 
            _context = context;
        }

        // Get all users 
        public async Task<IEnumerable<User>> GetAllUsersAsync() 
        { 
            return await _context.Users.ToListAsync();
        }

        // Get user by id
        public async Task<User?> GetUserByIdAsync(int id) 
        { 
            return await _context.Users.FindAsync(id);
        }

        // Create a user
        public async Task<User?> CreateAUserAsync(User newUser, string roleName) 
        {
            // Look up the role by name
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == roleName.ToLower());

            if (role == null) return null; // Return null if the role doesn't exist

            // Assign RoleId based on the found role
            newUser.RoleId = role.Id;

            // Save user to database
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        // Update full user PUT
        public async Task<User?> UpdateFullUserAsync(UserToUpdateDto userToUpdate, int id) 
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null) return null;

            // Update fields only if they are not null
            if (!string.IsNullOrWhiteSpace(userToUpdate.Name))
                existingUser.Name = userToUpdate.Name;

            if (!string.IsNullOrWhiteSpace(userToUpdate.Email))
                existingUser.Email = userToUpdate.Email;

            if (!string.IsNullOrWhiteSpace(userToUpdate.PhoneNumber))
                existingUser.PhoneNumber = userToUpdate.PhoneNumber;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        // Update part of a user PATCH
        public async Task<User?> UpdatePartialUser(UserToUpdateDto userToUpdate, int id) 
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null) return null;

            // Only update fields that are provided
            if (!string.IsNullOrWhiteSpace(userToUpdate.Name))
                existingUser.Name = userToUpdate.Name;

            if (!string.IsNullOrWhiteSpace(userToUpdate.Email))
                existingUser.Email = userToUpdate.Email;

            if (!string.IsNullOrWhiteSpace(userToUpdate.PhoneNumber))
                existingUser.PhoneNumber = userToUpdate.PhoneNumber;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        // Delete user
        public async Task<bool> DeleteUserAsync(int id) 
        {
            var userToDelete = await _context.Users.FindAsync(id);
            if (userToDelete == null) return false;

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}