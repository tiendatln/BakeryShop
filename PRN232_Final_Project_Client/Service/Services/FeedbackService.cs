using DTOs.FeedbackDTO;
using Service.BaseService;
using Service.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;
namespace Service.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly HttpClient _client;
        public FeedbackService(GatewayHttpClient gateway) => _client = gateway.Client;

        private void AddBearerToken(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<ReadFeedbackDTO>> GetAllAsync(string token)
        {
            AddBearerToken(token);
            var result = await _client.GetFromJsonAsync<List<ReadFeedbackDTO>>("/feedbacks");
            return result ?? new List<ReadFeedbackDTO>();
        }

        public async Task<ReadFeedbackDTO?> GetByUserIdAsync(string token)
        {
            AddBearerToken(token);
            return await _client.GetFromJsonAsync<ReadFeedbackDTO>($"/feedbacks/info");
        }

        public async Task<bool> CreateAsync(CreateFeedbackDTO dto, string token)
        {
            AddBearerToken(token);
            var resp = await _client.PostAsJsonAsync($"/feedbacks/add", dto);
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(UpdateFeedbackDTO dto, string token)
        {
            AddBearerToken(token);
            var resp = await _client.PutAsJsonAsync($"/feedbacks/up/{dto.FeedbackID}", dto);
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string token)
        {
            AddBearerToken(token);
            var resp = await _client.DeleteAsync($"/feedbacks/del");
            return resp.IsSuccessStatusCode;
        }
    
    }
}