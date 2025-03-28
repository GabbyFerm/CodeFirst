using Microsoft.EntityFrameworkCore;
using CodeFirst.Controllers;
using CodeFirst.Models;
using CodeFirst.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> CreateAUserAsync(User user, string roleName);
        Task<User?> UpdateFullUserAsync(UserToUpdateDto userToUpdate, int id);
        Task<User?> UpdatePartialUser(UserToUpdateDto userToUpdate, int id);
        Task<bool> DeleteUserAsync(int id);
    }
}