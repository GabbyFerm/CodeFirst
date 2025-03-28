using AutoMapper;
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
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IUserService _userService;

        private readonly UserToUpdateDtoValidator _userToUpdateDtoValidator;
        private readonly UserToPatchDtoValidator _userToPatchDtoValidator;

        public UserController(IMapper mapper, IUserService userService, UserToUpdateDtoValidator userToUpdateDtoValidator, UserToPatchDtoValidator userToPatchDtoValidator)
        {
            _mapper = mapper;
            _userService = userService;
            _userToUpdateDtoValidator = userToUpdateDtoValidator;
            _userToPatchDtoValidator = userToPatchDtoValidator;
        }

        // GET: api/<UserController>
        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsersWithSafeInfo()
        {
            var usersFromDb = await _userService.GetAllUsersAsync();

            var safeUsersToReturn = usersFromDb.Select(user => _mapper.Map<UserWithSafeInfoDto>(user));

            return Ok(safeUsersToReturn);
        }

        // GET api/<UserController>/5
        [HttpGet("get-user-by-{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            var safeUser = _mapper.Map<UserWithSafeInfoDto>(user);

            return Ok(safeUser);
        }

        // POST api/<UserController>
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateAUser([FromBody] UserToCreateDto userToCreate)
        {
            if (userToCreate == null) return BadRequest("Invalid user data.");

            var newUser = _mapper.Map<User>(userToCreate);

            var createdUser = await _userService.CreateAUserAsync(newUser, userToCreate.RoleName);

            if (createdUser == null) return BadRequest("Invalid RoleName. The role does not exist, choose Admin or User.");

            return Ok(createdUser);
        }

        // PUT api/<UserController>/5
        [HttpPut("update-all-user-info-by-{id}")]
        public async Task<IActionResult> UpdateFullUserInfo(int id, [FromBody] UserToUpdateDto userToUpdate)
        {
            // Validate the DTO using the validator
            var validationResult = await _userToUpdateDtoValidator.ValidateAsync(userToUpdate);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var updatedUser = await _userService.UpdateFullUserAsync(userToUpdate, id);
            if (updatedUser == null) return NotFound();

            return Ok(updatedUser);
        }

        // PATCH api/<UserController>/5
        [HttpPatch("update-partial-user-info-by-{id}")]
        public async Task<IActionResult> UpdatePartialUser(int id, [FromBody] UserToUpdateDto userToUpdate)
        {
            // Use PATCH-specific validator
            var validationResult = await _userToPatchDtoValidator.ValidateAsync(userToUpdate);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var upsatedUser = await _userService.UpdatePartialUser(userToUpdate, id);
            if (upsatedUser == null) return NotFound();

            return Ok(upsatedUser);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("delete-user-by-{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var isDeleted = await _userService.DeleteUserAsync(id);
            if (!isDeleted) return NotFound();

            return Ok();
        }
    }
}
