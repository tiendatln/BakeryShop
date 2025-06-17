namespace AuthAPI.DTOs
{
    public class UserValidateDTO
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
