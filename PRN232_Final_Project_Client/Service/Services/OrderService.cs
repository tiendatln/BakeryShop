using DTOs.OrderDTO;
using Service.BaseService;
using Service.Interfaces;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(GatewayHttpClient gateway)
        {
            _httpClient = gateway.Client;
        }

        private void AddBearerToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = null; // clear cũ
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /*public async Task<List<ReadOrderDTO>> GetOrdersAsync(string token, int skip = 0, int top = 8)
        {
            AddBearerToken(token);
            var url = $"/OrderHistory/queryable?$skip={skip}&$top={top}&lastest=true";
            var orders = await _httpClient.GetFromJsonAsync<List<ReadOrderDTO>>(url);
            return orders ?? new List<ReadOrderDTO>();
        }*/

        public async Task<string> GetOrdersAsync(string token, int skip = 0, int top = 8)
        {
            try
            {
                AddBearerToken(token);

                // Tạo query string với OData parameters
                var queryString = $"?$count=true&$top={top}&$skip={skip}&$expand=OrderDetails";
                var response = await _httpClient.GetAsync($"/OrderHistory/me{queryString}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new HttpRequestException($"API call failed with status {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calling order history API: {ex.Message}", ex);
            }
        }


        // get orderDetails detail with token 
        public async Task<List<ReadOrderDetailDTO>> GetOrderDetailAsync(int orderId, string token, int skip = 0, int top = 8)
        {
            AddBearerToken(token);
            var url = $"/OrderHistory/details/{orderId}?$skip={skip}&$top={top}";
            var orderDetails = await _httpClient.GetFromJsonAsync<List<ReadOrderDetailDTO>>(url);
            return orderDetails ?? new List<ReadOrderDetailDTO>();
        }
    }
}
