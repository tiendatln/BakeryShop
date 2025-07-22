using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.ProductDTO;

namespace Service.Interfaces
{
    public interface IProductService
    {
        public Task<List<ReadProductDTO>> GetAllProductsAsync();

        public Task<ReadProductDTO> GetProductByIdAsync(int productId);

        public Task<ReadProductDTO> CreateProductAsync(CreateProductDTO productDto, Stream stream, string token);

        public Task<UpdateProductDTO> UpdateProductAsync(UpdateProductDTO productDto, Stream imageStream, string token);

        public Task DeleteProductAsync(int productId, string token);

        public Task<List<ReadProductDTO>> GetProductsByCategoryAsync(int categoryId);

        public Task<List<ReadProductDTO>> SearchProductsAsync(string searchTerm);

        public Task<List<ReadProductDTO>> GetProductPage(int take, int skip);
        public Task<bool> UpdateProductQuantity(int id, int quantity, string token);
        public Task<string> SearchProductsOdataAsync(string searchTerm, int categoryID, int status, double minPrice, double maxPrice, int take, int skip);
    }
}