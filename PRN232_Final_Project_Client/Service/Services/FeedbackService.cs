using CartAPI.DTOs;
using DTOs.FeedbackDTO;
using Service.BaseService;
using Service.Interfaces;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Service.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly HttpClient _httpClient;

        public FeedbackService(GatewayHttpClient gateway)
        {
            _httpClient = gateway.Client;
        }

        // Gắn token vào Authorization header
        private void AddBearerToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<ReadFeedbackDTO>> GetAllAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.GetFromJsonAsync<List<ReadFeedbackDTO>>("/feedback");
            return result;
        }
        public async Task<ReadFeedbackDTO> GetByIdAsync(int id, string token)
        {
            AddBearerToken(token);
            return await _httpClient.GetFromJsonAsync<ReadFeedbackDTO>($"/feedback/{id}");
        }

        public async Task<bool> CreateAsync(CreateFeedbackDTO feedback, string token)
        {
            AddBearerToken(token);
            var response = await _httpClient.PostAsJsonAsync("/feedback", feedback);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdateFeedbackDTO feedback, string token)
        {
            AddBearerToken(token);
            var response = await _httpClient.PutAsJsonAsync($"/feedback/{id}", feedback);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id, string token)
        {
            AddBearerToken(token);
            var response = await _httpClient.DeleteAsync($"/feedback/{id}");
            return response.IsSuccessStatusCode;
        }
            
        ////test Cart API 
        //public async Task<List<CartDTO>> GetCartAsync(string token)
        //{
        //    AddBearerToken(token);
        //    var carts = await _httpClient.GetFromJsonAsync<List<CartDTO>>("/cart/me");
        //    return carts ?? new List<CartDTO>();
        //}
    }
}
