using DTOs.UserDTO;
using Service.BaseService;
using Service.Interfaces;
using System.Net.Http.Json;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(GatewayHttpClient gateway)
        {
            _httpClient = gateway.Client;
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var loginRequest = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("/auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return token; // Trả về token nếu đăng nhập thành công
            }
            else
            {
                // Xử lý lỗi đăng nhập
                return null;
            }
        }
    }
}
