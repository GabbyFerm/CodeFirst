using AutoMapper;
using CodeFirst.Database;
using CodeFirst.Dtos;
using CodeFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Services
{
    public class AddressService : IAddressService
    {
        private readonly CodeFirstDB _context;

        private readonly IMapper _mapper;

        public AddressService(CodeFirstDB context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get all addresses
        public async Task<IEnumerable<Address>> GetAllAddressesAsync() 
        { 
            return await _context.Addresses.ToListAsync();
        }

        // Get address by id
        public async Task<AddressDto?> GetAddressByIdAsync(int id)
        {
            var addressInDb = await _context.Addresses.FindAsync(id);

            if (addressInDb == null) return null;

            return _mapper.Map<AddressDto>(addressInDb); 
        }

        // Create address
        public async Task<Address?> CreateAddressAsync(AddressToCreateDto addressToCreate) 
        {
            var newAddress = _mapper.Map<Address>(addressToCreate); 

            _context.Addresses.Add(newAddress);
            await _context.SaveChangesAsync();

            return newAddress; 
        }

        // Update address
        public async Task<AddressDto?> UpdateAddressAsync(AddressDto addressToUpdate, int id)
        {
            var existingAddress = await _context.Addresses.FirstOrDefaultAsync(address => address.Id == id);

            if (existingAddress == null) return null;

            _mapper.Map(addressToUpdate, existingAddress);
            await _context.SaveChangesAsync();

            return _mapper.Map<AddressDto>(existingAddress);
        }

        // Delete address
        public async Task<bool> DeleteAddressAsync(int id) 
        { 
            var addressToDelete = await _context.Addresses.FindAsync(id);
            if (addressToDelete == null) return false;

            _context.Addresses.Remove(addressToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}