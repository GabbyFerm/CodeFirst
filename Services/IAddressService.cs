using Microsoft.EntityFrameworkCore;
using CodeFirst.Controllers;
using CodeFirst.Models;
using CodeFirst.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.Services
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAllAddressesAsync();
        Task<AddressDto?> GetAddressByIdAsync(int id);
        Task<Address?> CreateAddressAsync(AddressToCreateDto addressToCreate);
        Task<AddressDto?> UpdateAddressAsync(AddressDto addressToUpdate, int id);
        Task<bool> DeleteAddressAsync(int id);
    }
}