namespace UserAPI.DTOs
{
    public class UserValidateResultDTO
    {
        public bool IsValid { get; set; }

        public string? ErrorMessage { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Role { get; set; } = "None"; // hoặc "Admin", "Customer"

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
