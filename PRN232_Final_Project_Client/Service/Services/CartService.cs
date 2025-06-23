using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CartAPI.DTOs;
using Service.BaseService;
using Service.Interfaces;

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
            _httpClient.DefaultRequestHeaders.Authorization = null; // clear cũ
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<CartDTO>> GetCartAsync(string token)
        {
            AddBearerToken(token);
            var carts = await _httpClient.GetFromJsonAsync<List<CartDTO>>("/cart/me");
            return carts ?? new List<CartDTO>();
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
