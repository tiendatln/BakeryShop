using ProductAndCategoryAPI.Models;

namespace ProductAndCategoryAPI.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(Category category);
       
        Task<bool> DeleteCategoryAsync(int id);

        Task<bool> CategoryExistsAsync(int id);
        Task<bool> CategoryNameExistsAsync(string categoryName, int? excludeId = null);
        Task<IEnumerable<Category>> SearchCategoriesAsync(string searchTerm); 
        IQueryable<Category> GetAvailableCategoriesAsync();
        Task<Category?> UpdateCategoryAsync(int id, Category category); 
        Task<Category> UpdateCategoryAsync(Category existingCategory);  

    }
}
