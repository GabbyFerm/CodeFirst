using CodeFirst.Dtos;
using CodeFirst.Models;
using CodeFirst.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CodeFirst.Validators;
using CodeFirst.Services;

namespace CodeFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        // GET: api/<AddressController>
        [HttpGet("get-all-addresses")]
        public async Task<IActionResult> GetAllAddresses()
        {
            var allAddressesFromDb = await _addressService.GetAllAddressesAsync();

            return Ok(allAddressesFromDb);
        }

        // GET api/<AddressController>/5
        [HttpGet("get-address-by-{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var addressDto = await _addressService.GetAddressByIdAsync(id); 

            if (addressDto == null) return NotFound();

            return Ok(addressDto);
        }

        // POST api/<AddressController>
        [HttpPost("create-address")]
        public async Task<IActionResult> CreateAddress([FromBody] AddressToCreateDto addressToCreate)
        {
            if (addressToCreate == null) return BadRequest("Address data is required");

            var createdAddress = await _addressService.CreateAddressAsync(addressToCreate);

            if (createdAddress == null) return BadRequest("Failed to create address.");

            return Ok(createdAddress);
        }

        // PUT api/<AddressController>/5
        [HttpPut("update-address-by-{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] AddressDto addressToUpdate)
        {
            var updatedAddress = await _addressService.UpdateAddressAsync(addressToUpdate, id);

            if (updatedAddress == null) return NotFound();

            return Ok(updatedAddress);            
        }

        // DELETE api/<AddressController>/5
        [HttpDelete("delete-address-by-{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var isDeleted = await _addressService.DeleteAddressAsync(id);
            if(!isDeleted) return NotFound();

            return Ok();
        }
    }
}