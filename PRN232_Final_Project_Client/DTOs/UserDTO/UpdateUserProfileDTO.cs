using System.ComponentModel.DataAnnotations;

namespace DTOs.UserDTO
{
    public class UpdateUserProfileDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        [MaxLength(20)]
        public string Role { get; set; } = "None";

        [MaxLength(200)]
        public string? Address { get; set; }

        [Phone]
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }
    }
}
