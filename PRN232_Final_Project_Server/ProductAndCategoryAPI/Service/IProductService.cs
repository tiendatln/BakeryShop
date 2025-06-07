using ProductAndCategoryAPI.DTOs;

namespace ProductAndCategoryAPI.Service
{
    public interface IProductService
    {
        Task<IEnumerable<ReadProductDTO>> GetAllProductsAsync();
        Task<ReadProductDTO> GetProductByIdAsync(int id);
        Task<ReadProductDTO> CreateProductAsync(CreateProductDTO productDto);
        Task<ReadProductDTO> UpdateProductAsync(int id, UpdateProductDTO productDto);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<ReadProductDTO>> GetProductsByCategoryIdAsync(int categoryId);
        Task<IEnumerable<ReadProductDTO>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<ReadProductDTO>> GetPageAsync(int pageNumber, int pageSize);

    }
}
