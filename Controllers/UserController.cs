using AutoMapper;
using CodeFirst.Dtos;
using CodeFirst.Models;
using CodeFirst.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CodeFirst.Validators;


namespace CodeFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly CodeFirstDB _context;

        private readonly UserToUpdateDtoValidator _userToUpdateDtoValidator;

        public UserController(IMapper mapper, CodeFirstDB context, UserToUpdateDtoValidator userToUpdateDtoValidator)
        {
            _mapper = mapper;
            _context = context;
            _userToUpdateDtoValidator = userToUpdateDtoValidator;
        }

        // GET: api/<UserController>
        [HttpGet("get-all-users")]
        public async Task<IEnumerable<UserWithSafeInfoDto>> GetAllUsersWithSafeInfo()
        {
            var usersFromDb = await _context.Users.ToListAsync();

            var safeUsersToReturn = new List<UserWithSafeInfoDto>();

            foreach (var user in usersFromDb) 
            { 
                var safeUser = _mapper.Map<UserWithSafeInfoDto>(user);
                safeUsersToReturn.Add(safeUser);
            }
            return safeUsersToReturn;
        }

        // GET api/<UserController>/5
        [HttpGet("get-user-by-{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            User? foundUser = await _context.Users.FirstOrDefaultAsync(userInDb => userInDb.Id == id);
            if (foundUser == null) return NotFound();

            var safeUser = _mapper.Map<UserWithSafeInfoDto>(foundUser);

            return Ok(safeUser);
        }

        // POST api/<UserController>
        [HttpPost("create-a-user")]
        public async Task<IActionResult> CreateAUser([FromBody] UserToCreateDto userToCreate)
        {
            if (userToCreate == null) return BadRequest("Invalid user data.");

            // Look up the RoleId based on RoleName
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == userToCreate.RoleName.ToLower());

            if (role == null) return BadRequest("Invalid RoleName. The role does not exist, choose Admin or User.");

            var newUser = _mapper.Map<User>(userToCreate);
            newUser.RoleId = role.Id;

            _context.Users.Add(newUser);

            await _context.SaveChangesAsync();

            return Ok(newUser);
        }

        // PUT api/<UserController>/5
        [HttpPut("update-all-user-info-by-{id}")]
        public async Task<IActionResult> UpdateFullUserInfo(int id, [FromBody] UserToUpdateDto userToUpdate)
        {
            // Validate the DTO using the validator
            var validationResult = await _userToUpdateDtoValidator.ValidateAsync(userToUpdate);

            if (!validationResult.IsValid)
            {
                // Return validation errors as BadRequest
                return BadRequest(validationResult.Errors);
            }

            // Find the user by ID
            var foundUser = await _context.Users.FindAsync(id);
            if (foundUser == null)
                return NotFound();

            // Update fields with valid data
            if (!string.IsNullOrWhiteSpace(userToUpdate.Name))
                foundUser.Name = userToUpdate.Name;

            if (!string.IsNullOrWhiteSpace(userToUpdate.Email))
                foundUser.Email = userToUpdate.Email;

            if (!string.IsNullOrWhiteSpace(userToUpdate.PhoneNumber))
                foundUser.PhoneNumber = userToUpdate.PhoneNumber;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok(foundUser);
        }

        // PATCH api/<UserController>/5
        [HttpPatch("update-partial-user-info-by-{id}")]
        public async Task<IActionResult> UpdatePartialUser(int id, [FromBody] UserToUpdateDto userToUpdate)
        {
            // Use PATCH-specific validator
            var validator = new UserToPatchDtoValidator();
            var validationResult = await validator.ValidateAsync(userToUpdate);

            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var foundUser = await _context.Users.FindAsync(id);
            if (foundUser == null) return NotFound();

            // Only update fields that are provided
            if (!string.IsNullOrWhiteSpace(userToUpdate.Name)) foundUser.Name = userToUpdate.Name;
            if (!string.IsNullOrWhiteSpace(userToUpdate.Email)) foundUser.Email = userToUpdate.Email;
            if (!string.IsNullOrWhiteSpace(userToUpdate.PhoneNumber)) foundUser.PhoneNumber = userToUpdate.PhoneNumber;

            await _context.SaveChangesAsync();

            return Ok(foundUser);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("delete-user-by-{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userToDelete = await _context.Users.FirstOrDefaultAsync(userInDb=>userInDb.Id == id);

            if(userToDelete == null) return NotFound();

            _context.Users.Remove(userToDelete);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
