using ProductAndCategoryAPI.DTOs;
using ProductAndCategoryAPI.Models;

namespace ProductAndCategoryAPI.Service
{
    public interface ICategoryService
    {
        Task<IEnumerable<ReadCategoryDTO>> GetAllCategoriesAsync();
        Task<ReadCategoryDTO?> GetCategoryByIdAsync(int id);
        Task<ReadCategoryDTO> CreateCategoryAsync(CreateCategoryDTO createCategoryDto);
        Task<ReadCategoryDTO?> UpdateCategoryAsync(int id, UpdateCategoryDTO updateCategoryDto);
        Task<bool> DeleteCategoryAsync(int id);
        Task<IEnumerable<ReadCategoryDTO>> SearchCategoriesAsync(string searchTerm);

        IQueryable<Category> GetAvailableCategoriesAsync();

    }
}
