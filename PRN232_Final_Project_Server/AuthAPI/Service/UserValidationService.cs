using AuthAPI.DTOs;
using AuthAPI.Service.Interface;
using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Service
{
    public class UserValidationService : IUserValidationService
    {
        private readonly HttpClient _httpClient;

        public UserValidationService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("UserAPI");
        }

        public async Task<UserValidateResultDTO> ValidateUserValidationAsync(UserValidateDTO userValidateDTO)
        {
            var userValidatResult = await _httpClient.PostAsJsonAsync("validate", userValidateDTO);

            if(!userValidatResult.IsSuccessStatusCode)
            {
                return new UserValidateResultDTO
                {
                    IsValid = false,
                    ErrorMessage = "Invalid credentials"
                };
            }

            var result = await userValidatResult.Content.ReadFromJsonAsync<UserValidateResultDTO>();
            return result!;
        }
    }
}
