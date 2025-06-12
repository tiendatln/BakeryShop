using ProductAndCategoryAPI.DTOs;
using ProductAndCategoryAPI.Models;

namespace ProductAndCategoryAPI.Service
{
    public interface IProductService
    {
        Task<IEnumerable<ReadProductDTO>> GetAllProductsAsync();
        Task<ReadProductDTO> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(CreateProductDTO productDto );
        Task<UpdateProductDTO> UpdateProductAsync(int id, UpdateProductDTO productDto);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<ReadProductDTO>> GetProductsByCategoryIdAsync(int categoryId);
        Task<IEnumerable<ReadProductDTO>> SearchProductsAsync(string searchTerm);

        IQueryable<Product> GetAvailableProductsAsync();
    }
}
