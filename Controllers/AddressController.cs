using AutoMapper;
using CodeFirst.Dtos;
using CodeFirst.Models;
using CodeFirst.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly CodeFirstDB _context;

        public AddressController(IMapper mapper, CodeFirstDB context)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET: api/<AddressController>
        [HttpGet("get-all-addresses")]
        public async Task<IActionResult> GetAllAddresses()
        {
            var allAddresses = await _context.Addresses.ToListAsync();
            return Ok(allAddresses);
        }

        // GET api/<AddressController>/5
        [HttpGet("get-address-by-{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var addressToReturn = await _context.Addresses.FirstOrDefaultAsync(addressInDb => addressInDb.Id == id);

            if (addressToReturn == null) return NotFound();

            var addressDto = _mapper.Map<AddressDto>(addressToReturn);

            return Ok(addressDto);
        }

        // POST api/<AddressController>
        [HttpPost("create-address")]
        public async Task<IActionResult> CreateAddress([FromBody] AddressToCreateDto addressToCreate)
        {
            if (addressToCreate == null) return BadRequest("Address data is required");

            var newAddress = _mapper.Map<Address>(addressToCreate);

            _context.Addresses.Add(newAddress);
            await _context.SaveChangesAsync();

            return Ok(newAddress);
        }

        // PUT api/<AddressController>/5
        [HttpPut("update-address-by-{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] AddressDto addressToUpdate)
        {
            var existingAddress = await _context.Addresses.FirstOrDefaultAsync(addressInDb => addressInDb.Id == id);

            if (existingAddress == null) return NotFound();

            _mapper.Map(addressToUpdate, existingAddress);
            await _context.SaveChangesAsync();

            return Ok(existingAddress);            
        }

        // DELETE api/<AddressController>/5
        [HttpDelete("delete-address-by-{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var addressToDelte = await _context.Addresses.FirstOrDefaultAsync(addressInDb =>addressInDb.Id == id);

            if(addressToDelte == null) return NotFound();

            _context.Addresses.Remove(addressToDelte);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
