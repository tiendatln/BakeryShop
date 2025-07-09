using Service.Services;
using DTOs.CategoryDTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Service.Interfaces;

namespace Service.Interfaces
{
    public interface ICategoryService
    {
        Task<List<ReadCategoryDTO>> GetAllCategoriesAsync();
        Task<ReadCategoryDTO> GetCategoryByIdAsync(int id);
        Task<bool> CreateCategoryAsync(CreateCategoryDTO dto, string token);
        Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDTO dto, string token);
        Task<bool> DeleteCategoryAsync(int id, string token);
        Task<List<ReadCategoryDTO>> SearchCategoriesAsync(string searchTerm);

        Task<List<ReadCategoryDTO>> SearchCategoriesOdataAsync(string searchTerm, int take, int skip);
    }
}
