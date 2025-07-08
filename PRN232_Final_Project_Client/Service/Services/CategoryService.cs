using DTOs.CategoryDTO;
using MailKit.Search;
using Service.BaseService;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(GatewayHttpClient gateway)
        {
            _httpClient = gateway.Client;
        }

        // Hàm tạo request có kèm token và body nếu cần
        private HttpRequestMessage CreateRequest(HttpMethod method, string url, string? token, object? content = null)
        {
            var request = new HttpRequestMessage(method, url);
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer");
            }
            if (content != null)
            {
                request.Content = JsonContent.Create(content);
            }
            return request;
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
            var request = new HttpRequestMessage(HttpMethod.Get, $"/categories/search?searchTerm={Uri.EscapeDataString(searchTerm)}");



            foreach (var header in request.Headers)
            {
                Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }

            var response = await _httpClient.SendAsync(request);



            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<ReadCategoryDTO>>() ?? new();
        }




        public async Task<bool> CreateCategoryAsync(CreateCategoryDTO dto, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsJsonAsync("/categories", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDTO dto, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsJsonAsync($"/categories/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCategoryAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"/categories/{id}");
            return response.IsSuccessStatusCode;
        }
        public async Task<List<ReadCategoryDTO>> SearchCategoriesOdataAsync(string searchTerm, int take, int skip)
        {
            List<string> filtersList = new List<string>();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filtersList.Add($"contains(CategoryName,'{searchTerm}')");
            }

            string query = "";

            if (filtersList.Count() > 0)
            {
                query += $"$filter={string.Join(" and ", filtersList)}";
            }

            if (take > 0)
            {
                query += $"{(query != "" ? "&" : "")}$top={take}&$skip={skip}";
            }


            // đúng endpoint /categories/Get
            var response = await _httpClient.GetAsync($"/categories/Get?{query}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ReadCategoryDTO>>() ?? new();
        }


    }
}
