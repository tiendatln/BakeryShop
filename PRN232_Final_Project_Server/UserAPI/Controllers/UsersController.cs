using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using UserAPI.Data;
using UserAPI.DTOs;
using UserAPI.Model;
using UserAPI.Service.Interface;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        // POST: api/Users/validate
        [HttpPost("validate")]
        public async Task<ActionResult<UserValidateResultDTO>> ValidateUser(UserValidateDTO userValidateDTO)
        {
            if (userValidateDTO == null)
            {
                return BadRequest("Invalid user validation data.");
            }

            var result = await _userService.ValidateUserAsync(userValidateDTO);
            if (!result.IsValid)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        // GET: api/Users
        [HttpGet]
        [EnableQuery]
        public IQueryable<ReadUserDTO> GetUsers()
        {
            return _userService.GetAllUsers();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadUserDTO>> GetUser(int id)
        {
            var readUserDTO = await _userService.GetUserByIdAsync(id);

            if (readUserDTO == null)
            {
                return NotFound();
            }

            return Ok(readUserDTO);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UpdateUserProfileDTO updateUserProfileDTO)
        {
            if (id != updateUserProfileDTO.UserId)
            {
                return BadRequest();
            }

            await _userService.UpdateUserAsync(updateUserProfileDTO);

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReadUserDTO>> PostUser(CreateUserDTO createUserDTO)
        {
            var newUser = await _userService.CreateUserAsync(createUserDTO);

            return CreatedAtAction("GetUser", new { id = newUser.UserId }, newUser);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

           await _userService.DeleteUserAsync(id);

            return NoContent();
        }
    }
}
