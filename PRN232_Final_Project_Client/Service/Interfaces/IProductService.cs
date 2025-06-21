using System;
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
        public Task<List<ReadProductDTO>> GetProductsByCategoryAsync(int categoryId);
        public Task<List<ReadProductDTO>> SearchProductsAsync(string searchTerm);

    }
}
