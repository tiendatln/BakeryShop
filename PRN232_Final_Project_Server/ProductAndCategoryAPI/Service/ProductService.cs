using AutoMapper;
using ProductAndCategoryAPI.DTOs;
using ProductAndCategoryAPI.Models;
using ProductAndCategoryAPI.Repositories;

namespace ProductAndCategoryAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRespository _productRepository;
        private readonly IMapper _mapper;
        public ProductService(IProductRespository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReadProductDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return _mapper.Map<IEnumerable<ReadProductDTO>>(products);
        }

        public async Task<ReadProductDTO> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null) return null;
            return _mapper.Map<ReadProductDTO>(product);
        }

        public async Task<Product> CreateProductAsync(CreateProductDTO createProductDto)
        {
            var product = _mapper.Map<Product>(createProductDto);
            var createdProduct = await _productRepository.CreateProductAsync(product);
            return _mapper.Map<Product>(createdProduct);
        }

        public async Task<UpdateProductDTO> UpdateProductAsync(int id, UpdateProductDTO updateProductDto)
        {
            var product = _mapper.Map<Product>(updateProductDto);
            if (updateProductDto != null && updateProductDto.ImageURL.Length > 0)
            {
                // Save the image to a specific path, e.g., wwwroot/images
                var existingProduct = await _productRepository.GetProductByIdAsync(id);
                var folderPathExis = Path.Combine(Directory.GetCurrentDirectory());
                var filePathExis = Path.Combine(folderPathExis, existingProduct.ImageURL);


                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "img");
                Directory.CreateDirectory(folderPath);
                var fileName = Path.GetFileName(updateProductDto.ImageURL.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                if (File.Exists(filePath)) { }
                else
                {
                    File.Delete(filePathExis); // Delete the old image file if it exists
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await updateProductDto.ImageURL.CopyToAsync(stream);
                    }
                }

                product.ImageURL = $"img/{fileName}"; // Set the image URL
            }
            else
            {
                product.ImageURL = string.Empty; // Set default or empty if no image provided
            }
            var updatedProduct = await _productRepository.UpdateProductAsync(id, product);
            if (updatedProduct == null) return null;
            return _mapper.Map<UpdateProductDTO>(updatedProduct);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            // Save the image to a specific path, e.g., wwwroot/images
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "ProductAndCategoryAPI");
            var product = await _productRepository.GetProductByIdAsync(id);
            var filePath = Path.Combine(folderPath, product.ImageURL);
            if (File.Exists(filePath))
            {
                File.Delete(filePath); // Delete the image file if it exists
            }
            return await _productRepository.DeleteProductAsync(id);
        }
        public async Task<IEnumerable<ReadProductDTO>> GetProductsByCategoryIdAsync(int categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryIdAsync(categoryId);
            return _mapper.Map<IEnumerable<ReadProductDTO>>(products);
        }
        public async Task<IEnumerable<ReadProductDTO>> SearchProductsAsync(string searchTerm)
        {
            var products = await _productRepository.SearchProductsAsync(searchTerm);
            return _mapper.Map<IEnumerable<ReadProductDTO>>(products);
        }


        public IQueryable<Product> GetAvailableProductsAsync()
        {
            return _productRepository.GetAvailableProductsAsync();
        }
    }
}
