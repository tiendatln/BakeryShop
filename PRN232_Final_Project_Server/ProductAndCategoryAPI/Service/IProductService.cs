using ProductAndCategoryAPI.DTOs;
using ProductAndCategoryAPI.Models;

namespace ProductAndCategoryAPI.Service
{
    public interface IProductService
    {
        Task<IEnumerable<ReadProductDTO>> GetAllProductsAsync();
        Task<ReadProductDTO> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(CreateProductDTO productDto,string ImageFile);
        Task<Product> UpdateProductAsync(int id, UpdateProductDTO productDto, string ImageURL);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<ReadProductDTO>> GetProductsByCategoryIdAsync(int categoryId);
        Task<IEnumerable<ReadProductDTO>> SearchProductsAsync(string searchTerm);
        Task<bool> UpdateQuantityAsync(int id, int quantity);
        IQueryable<Product> GetAllProductForOData();
        IQueryable<ReadProductDTO> GetAllProduct(); 
    }
}
