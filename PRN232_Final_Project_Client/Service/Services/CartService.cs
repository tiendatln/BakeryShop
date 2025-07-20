using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using DTOs.CartDTO;
using Service.BaseService;
using Service.Interfaces;

// Trong Service/Services/CartService.cs
namespace Service.Services
{
    public class CartService : ICartService
    {
        private readonly HttpClient _httpClient;

        public CartService(GatewayHttpClient gateway)
        {
            _httpClient = gateway.Client;
        }

        private void AddBearerToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Giữ lại hàm này nếu bạn vẫn muốn có lựa chọn lấy toàn bộ giỏ hàng không phân trang ở đâu đó.
        // Nếu không, bạn có thể xóa nó.
        public async Task<List<CartDTO>> GetCartAsync(string token)
        {
            AddBearerToken(token);
            var carts = await _httpClient.GetFromJsonAsync<List<CartDTO>>("/cart/me");
            return carts ?? new List<CartDTO>();
        }

        public async Task<List<CartDTO>> GetCartAsync(string token, int skip, int take)
        {
            AddBearerToken(token);
            var carts = await _httpClient.GetFromJsonAsync<List<CartDTO>>($"/cart/me?$skip={skip}&$top={take}&$orderby=LastUpdated desc");
            return carts ?? new List<CartDTO>();
        }

        public async Task<int> GetCartCountAsync(string token) // <-- THÊM HÀM NÀY
        {
            AddBearerToken(token);
            // Gọi một endpoint mới trên API Gateway để lấy số lượng
            int count = await _httpClient.GetFromJsonAsync<int>("/cart/count");
            return count;
        }

        public async Task<bool> AddCartAsync(CartCreateDTO dto, string token)
        {
            AddBearerToken(token);
            var response = await _httpClient.PostAsJsonAsync("/cart/add", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCartAsync(int cartId, string token)
        {
            AddBearerToken(token);
            var response = await _httpClient.DeleteAsync($"/cart/{cartId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateQuantitiesAsync(List<CartQuantityUpdateDTO> updates, string token)
        {
            AddBearerToken(token);
            var response = await _httpClient.PutAsJsonAsync("/cart/update-quantities", updates);
            return response.IsSuccessStatusCode;
        }
    }
}