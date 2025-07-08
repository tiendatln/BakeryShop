using System.ComponentModel.DataAnnotations;

namespace UserAPI.DTOs
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(3)]
        public string NewPassword { get; set; } = null!;
    }
}
