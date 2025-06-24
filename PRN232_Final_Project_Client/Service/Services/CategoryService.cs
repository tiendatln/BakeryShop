using DTOs.UserDTO;

using DTOs.CategoryDTO;
using Service.BaseService;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(GatewayHttpClient gateway)
        {
            _httpClient = gateway.Client;
        }

        private void AddAuthHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<ReadCategoryDTO>> GetAllCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("/categories");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ReadCategoryDTO>>() ?? new();
        }

        public async Task<ReadCategoryDTO> GetCategoryByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/categories/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReadCategoryDTO>();
        }

        public async Task<List<ReadCategoryDTO>> SearchCategoriesAsync(string searchTerm)
        {
            var response = await _httpClient.GetAsync($"/categories/search?searchTerm={Uri.EscapeDataString(searchTerm)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ReadCategoryDTO>>() ?? new();
        }

        public async Task<bool> CreateCategoryAsync(CreateCategoryDTO dto, string token)
        {
            AddAuthHeader(token);
            var response = await _httpClient.PostAsJsonAsync("/categories", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDTO dto, string token)
        {
            AddAuthHeader(token);
            var response = await _httpClient.PutAsJsonAsync($"/categories/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCategoryAsync(int id, string token)
        {
            AddAuthHeader(token);
            var response = await _httpClient.DeleteAsync($"/categories/{id}");
            return response.IsSuccessStatusCode;
        }
    }


}
