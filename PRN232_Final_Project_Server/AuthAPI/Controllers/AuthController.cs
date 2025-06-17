using AuthAPI.DTOs;
using AuthAPI.Service;
using AuthAPI.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserValidationService _userValidationService;
        private readonly TokenService _tokenService;

        public AuthController(IUserValidationService userValidationService, TokenService tokenService)
        {
            _userValidationService = userValidationService ?? throw new ArgumentNullException(nameof(userValidationService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserValidateDTO userValidateDTO)
        {
            if (userValidateDTO == null)
            {
                return BadRequest("Invalid user validation data.");
            }

            var validationResult = await _userValidationService.ValidateUserValidationAsync(userValidateDTO);
            if (validationResult == null || !validationResult.IsValid)
            {
                return Unauthorized(validationResult);
            }

            var token = _tokenService.GenerateToken(validationResult);
            return Ok(new { Token = token });
        }
    }
}
