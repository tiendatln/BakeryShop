using AutoMapper;
using ProductAndCategoryAPI.DTOs;
using ProductAndCategoryAPI.Models;
using ProductAndCategoryAPI.Repositories;

namespace ProductAndCategoryAPI.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReadCategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return _mapper.Map<IEnumerable<ReadCategoryDTO>>(categories);
        }
        public async Task<IEnumerable<ReadCategoryDTO>> SearchCategoriesAsync(string searchTerm)
        {
            var categories = await _categoryRepository.SearchCategoriesAsync(searchTerm);
            return _mapper.Map<IEnumerable<ReadCategoryDTO>>(categories);
        }

        public IQueryable<Category> GetAvailableCategoriesAsync()
        {
            return _categoryRepository.GetAvailableCategoriesAsync();
        }

        public async Task<ReadCategoryDTO?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null) return null;
            return _mapper.Map<ReadCategoryDTO>(category);
        }

        public async Task<ReadCategoryDTO> CreateCategoryAsync(CreateCategoryDTO createCategoryDto)
        {
            if (await _categoryRepository.CategoryNameExistsAsync(createCategoryDto.CategoryName))
            {
                throw new InvalidOperationException("Category name already exists");
            }

            var category = _mapper.Map<Category>(createCategoryDto);
            var createdCategory = await _categoryRepository.CreateCategoryAsync(category);
            return _mapper.Map<ReadCategoryDTO>(createdCategory);
        }

        public async Task<ReadCategoryDTO?> UpdateCategoryAsync(int id, UpdateCategoryDTO updateCategoryDto)
        {
            var existingCategory = await _categoryRepository.GetCategoryByIdAsync(id);
            if (existingCategory == null)
                return null;

            if (await _categoryRepository.CategoryNameExistsAsync(updateCategoryDto.CategoryName, id))
            {
                throw new InvalidOperationException("Category name already exists");
            }

            _mapper.Map(updateCategoryDto, existingCategory);
            var updatedCategory = await _categoryRepository.UpdateCategoryAsync(existingCategory);
            return _mapper.Map<ReadCategoryDTO>(updatedCategory);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            return await _categoryRepository.DeleteCategoryAsync(id);
        }
    }
}
