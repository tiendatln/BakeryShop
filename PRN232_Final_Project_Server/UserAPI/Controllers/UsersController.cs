using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Humanizer;
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

        // Reset password
        // POST: api/Users/reset-password
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            if (resetPasswordDTO == null)
            {
                return BadRequest("Invalid reset password data.");
            }

            // check email exists
            var checkEmailExist = await _userService.IsEmailExistsAsync(resetPasswordDTO.Email);
            if (!checkEmailExist)
            {
                return NotFound("Email does not exist.");
            }

            // Reset password
            var result = await _userService.ResetPasswordAsync(resetPasswordDTO);
            if(result == false)
            {
                return BadRequest("Failed to reset password. Please check your email or new password.");
            }

            return Ok("Password reset successfully.");
        }

        // OData sẽ tự động tìm phương thức này để xử lý GET cho EntitySet "ODataUsers"
        // Bạn không cần [HttpGet] ở đây nữa vì ODataController sẽ xử lý.
        [HttpGet("Get")] // api/Users/Get
        public IActionResult GetAndCount(int top, int skip) // Tên phương thức là "Get"
        {
            var readUserDTO = _userService.GetAllUsers();
            var count = readUserDTO.Count();

            // Phân trang
            if (skip < 0 || top <= 0)
            {
                return BadRequest("Invalid pagination parameters.");
            }

            readUserDTO = readUserDTO.Skip(skip).Take(top);

            return Ok(new
            {
                data = readUserDTO,
                totalUser = count
            });
        }

        // POST: api/Users/validate
        // Dùng để xác thực người dùng khi đăng nhập gọi từ AuthAPI
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

        [HttpGet("emailExit")]
        public async Task<ActionResult<bool>> IsEmailExists( [FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email cannot be null or empty.");
            }

            var exists = await _userService.IsEmailExistsAsync(email);
            return Ok(exists);
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

        // PUT: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutUser(UpdateUserProfileDTO updateUserProfileDTO)
        {
            // lấy userId từ token để kiểm tra quyền cập nhật và đảm bảo người dùng chỉ có thể cập nhật thông tin của chính mình
            var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (userIdFromToken != updateUserProfileDTO.UserId)
            {
                return Forbid(); // Người dùng không được phép cập nhật thông tin của người khác
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
