using UserAPI.DTOs;
using UserAPI.Model;

namespace UserAPI.Service.Interface
{
    public interface IUserService
    {
        IQueryable<ReadUserDTO> GetAllUsers();
        IQueryable<User> GetAllUsersForOData();

        Task<bool> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);

        Task<UserValidateResultDTO> ValidateUserAsync(UserValidateDTO userValidateDTO);

        Task<ReadUserDTO> GetUserByIdAsync(int id);

        Task<ReadUserDTO> CreateUserAsync(CreateUserDTO createUserDTO);

        Task<ReadUserDTO> UpdateUserAsync(UpdateUserProfileDTO updateUserProfileDTO);

        Task<bool> DeleteUserAsync(int id);

        Task<bool> IsEmailExistsAsync(string email);
    }
}
