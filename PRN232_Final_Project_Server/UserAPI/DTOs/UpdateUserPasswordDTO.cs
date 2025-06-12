using System.ComponentModel.DataAnnotations;

namespace UserAPI.DTOs
{
    public class UpdateUserPasswordDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MinLength(6)]
        public string CurrentPassword { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = null!;
    }
}
