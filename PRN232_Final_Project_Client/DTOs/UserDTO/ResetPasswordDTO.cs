using System.ComponentModel.DataAnnotations;

namespace DTOs.UserDTO
{
    public class ResetPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(3)]
        public string NewPassword { get; set; } = null!;
    }
}
