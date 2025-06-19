namespace Service.Interfaces
{
    public interface IUserService
    {
        Task<string?> LoginAsync(string email, string password);
        Task<bool> CheckUserExists(string email);
        Task<bool> RegisterAsync(string fullName, string email, string address, string phone, string password);
    }
}
