using UserAPI.DTOs;
using UserAPI.Model;

namespace UserAPI.Service.Interface
{
    public interface IUserService
    {
        Task<UserValidateResultDTO> ValidateUserAsync(UserValidateDTO userValidateDTO);

        Task<ReadUserDTO> GetUserByIdAsync(int id);

        Task<ReadUserDTO> CreateUserAsync(CreateUserDTO createUserDTO);
        Task<ReadUserDTO> UpdateUserAsync(UpdateUserProfileDTO updateUserProfileDTO);
        Task<bool> DeleteUserAsync(int id);
        IQueryable<ReadUserDTO> GetAllUsers();
        Task<bool> IsEmailExistsAsync(string email);
    }
}
