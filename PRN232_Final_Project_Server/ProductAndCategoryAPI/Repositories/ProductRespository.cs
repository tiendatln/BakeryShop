using Microsoft.EntityFrameworkCore;
using ProductAndCategoryAPI.Data;
using ProductAndCategoryAPI.Models;

namespace ProductAndCategoryAPI.Repositories
{
    public class ProductRespository : IProductRespository
    {

        private readonly ProductAndCategoryDbContext _context;
        public ProductRespository(ProductAndCategoryDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category) // Include Category details if needed
                .FirstOrDefaultAsync(p => p.ProductID == id);
            return product;
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateProductAsync(int id, Product product)
        {
            product.ProductID = id; // Ensure the ID is set correctly
            _context.ChangeTracker.Clear();
            _context.Update(product);
            var existingProduct = await _context.Products.FindAsync(id);
            await _context.SaveChangesAsync();
            return existingProduct;
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await GetProductByIdAsync(id);
            if (product == null) return false;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _context.Products.Where(p => p.CategoryID == categoryId).ToListAsync();
        }
        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            return await _context.Products
                .Where(p => p.ProductName.Contains(searchTerm)).ToListAsync();
        }


        public IQueryable<Product> GetAvailableProductsAsync()
        {
            return _context.Products.AsQueryable();
        }
    }
}
