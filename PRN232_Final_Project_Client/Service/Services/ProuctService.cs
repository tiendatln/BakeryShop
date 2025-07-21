using DTOs.ProductDTO;
using Service.BaseService;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ProuctService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProuctService(GatewayHttpClient gateway)
        {
            _httpClient = gateway.Client;
        }

        public async Task<List<ReadProductDTO>> GetAllProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/products");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<List<ReadProductDTO>>();
                    return data ?? new List<ReadProductDTO>(); // tránh null
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    // Ghi log chi tiết lỗi
                    Console.WriteLine($"API lỗi: {(int)response.StatusCode} - {response.ReasonPhrase}");
                    Console.WriteLine($"Nội dung lỗi: {error}");

                    // Tuỳ chọn: ném lỗi hoặc trả về list rỗng
                    throw new HttpRequestException($"Lỗi API: {(int)response.StatusCode} - {error}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Lỗi HTTP như không kết nối được, timeout, DNS fail,...
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Lỗi không xác định
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }

        public async Task<ReadProductDTO> CreateProductAsync(CreateProductDTO productDto, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsJsonAsync("/products", productDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReadProductDTO>();
        }

        //Update
        public async Task<UpdateProductDTO> UpdateProductAsync(UpdateProductDTO productDto, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync($"/products/{productDto.ProductID}", productDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UpdateProductDTO>();
        }

        //Delete
        public async Task DeleteProductAsync(int productId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"/products/{productId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<ReadProductDTO> GetProductByIdAsync(int productId)
        {
            var response = await _httpClient.GetAsync($"/products/{productId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReadProductDTO>();
        }

        public async Task<List<ReadProductDTO>> GetProductsByCategoryAsync(int categoryId)
        {
            var response = await _httpClient.GetAsync($"/products/category/{categoryId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ReadProductDTO>>();
        }

        //Search
        public async Task<List<ReadProductDTO>> SearchProductsAsync(string searchTerm)
        {
            var response = await _httpClient.GetAsync($"/products/search?searchKey={Uri.EscapeDataString(searchTerm)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ReadProductDTO>>();
        }

        public async Task<List<ReadProductDTO>> GetProductPage(int take, int skip)
        {
            var response = await _httpClient.GetAsync($"/products/Get?$top={take}&$skip={skip}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ReadProductDTO>>();
        }

        public async Task<string> SearchProductsOdataAsync(string searchTerm, int categoryID, int status, double minPrice, double maxPrice, int take, int skip)
        {
            List<string> filtersList = new List<string>();

            if (categoryID != 0)
                filtersList.Add($"CategoryID eq {categoryID}");
            if (status < 0)
                filtersList.Add($"IsAvailable eq {status}");
            if( 1 <= status && status <= 2)
                filtersList.Add($"IsAvailable eq {(status == 1 ? "true" : "false")}");

            if (!string.IsNullOrWhiteSpace(searchTerm))
                filtersList.Add($"contains(ProductName, '{searchTerm}')");
            if (minPrice > 0)
            {
                filtersList.Add($"Price gt {minPrice}");
            }
            if (maxPrice > 0)
            {
                filtersList.Add($"Price lt {maxPrice}");
            }
            


            var filters = "&?filter=";
            if (filtersList.Count > 0)
            {
                filters += string.Join(" and ", filtersList);
            }
            else
            {
                filters += "true"; // Trả về tất cả nếu không có bộ lọc nào
            }
            if(take > 0)
            filters += $"&?top={take}&?skip={skip}";
            var response = await _httpClient.GetAsync($"/products/odata?$count=true{filters}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }
    }
}