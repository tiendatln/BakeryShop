using UserAPI.Model;

namespace UserAPI.Repository.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);

        Task<User?> GetUserByIdAsync(int id);

        Task<User?> CreateUserAsync(User user);

        Task<User?> UpdateUserAsync(User user);

        Task<bool> DeleteUserAsync(int id);

        IQueryable<User> GetAllUsers();
    }
}
