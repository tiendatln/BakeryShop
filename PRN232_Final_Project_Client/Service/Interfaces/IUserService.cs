namespace Service.Interfaces
{
    public interface IUserService
    {
        Task<string?> LoginAsync(string email, string password);
    }
}
