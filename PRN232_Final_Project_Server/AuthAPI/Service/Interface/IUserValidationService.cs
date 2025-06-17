using AuthAPI.DTOs;

namespace AuthAPI.Service.Interface
{
    public interface IUserValidationService
    {
        Task<UserValidateResultDTO> ValidateUserValidationAsync(UserValidateDTO userValidateDTO);
    }
}
