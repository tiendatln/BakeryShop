using Microsoft.EntityFrameworkCore;
using ProductAndCategoryAPI.Data;
using ProductAndCategoryAPI.Models;

namespace ProductAndCategoryAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProductAndCategoryDbContext _context;

        public CategoryRepository(ProductAndCategoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryID == id);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(int id, Category category)
        {
            category.CategoryID = id;
            _context.ChangeTracker.Clear();
            _context.Update(category);

            var existingCategory = await _context.Categories.FindAsync(id);
            await _context.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            
            var hasProducts = await _context.Products.AnyAsync(p => p.CategoryID == id);
            if (hasProducts)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _context.Categories.AnyAsync(c => c.CategoryID == id);
        }

        public async Task<bool> CategoryNameExistsAsync(string categoryName, int? excludeId = null)
        {
            var query = _context.Categories
                .Where(c => c.CategoryName.ToLower() == categoryName.ToLower());

            if (excludeId.HasValue)
                query = query.Where(c => c.CategoryID != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string searchTerm)
        {
            return await _context.Categories
                .Where(c => c.CategoryName.Contains(searchTerm) || c.Description.Contains(searchTerm))
                .Include(c => c.Products)
                .ToListAsync();
        }

        public IQueryable<Category> GetAvailableCategoriesAsync()
        {
            return _context.Categories.AsQueryable();
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(category.CategoryID);
            if (existingCategory == null)
            {
                throw new ArgumentException("Category not found");
            }

            _context.Entry(existingCategory).CurrentValues.SetValues(category);
            await _context.SaveChangesAsync();

            return existingCategory;
        }


    }
}
