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
        private readonly IUserService _userService;

        private readonly UserToUpdateDtoValidator _userToUpdateDtoValidator;
        private readonly UserToPatchDtoValidator _userToPatchDtoValidator;

        public UserController(IUserService userService, UserToUpdateDtoValidator userToUpdateDtoValidator, UserToPatchDtoValidator userToPatchDtoValidator)
        {
            _userService = userService;
            _userToUpdateDtoValidator = userToUpdateDtoValidator;
            _userToPatchDtoValidator = userToPatchDtoValidator;
        }

        // GET: api/<UserController>
        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsersWithSafeInfo()
        {
            var usersFromDb = await _userService.GetAllUsersAsync();

            return Ok(usersFromDb);
        }

        // GET api/<UserController>/5
        [HttpGet("get-user-by-{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var userDto = await _userService.GetUserByIdAsync(id);

            if (userDto == null) return NotFound();

            return Ok(userDto);
        }

        // POST api/<UserController> 
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] UserToCreateDto userToCreate)
        {
            if (userToCreate == null) return BadRequest("Invalid user data.");

            var createdUser = await _userService.CreateUserAsync(userToCreate);

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
