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
        Task<UserWithSafeInfoDto?> GetUserByIdAsync(int id);
        Task<User?> CreateUserAsync(UserToCreateDto userToCreate);
        Task<User?> UpdateFullUserAsync(UserToUpdateDto userToUpdate, int id);
        Task<User?> UpdatePartialUser(UserToUpdateDto userToUpdate, int id);
        Task<bool> DeleteUserAsync(int id);
    }
}