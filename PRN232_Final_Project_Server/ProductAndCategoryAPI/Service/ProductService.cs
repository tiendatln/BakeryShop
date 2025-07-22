using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OData.UriParser;
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

        public async Task<Product> CreateProductAsync(CreateProductDTO createProductDto, string ImageFile)
        {
            var product = _mapper.Map<Product>(createProductDto);
            product.ImageURL = ImageFile; // Assuming ImageFile is the path to the image
            var createdProduct = await _productRepository.CreateProductAsync(product);
            return createdProduct;
        }

        public async Task<Product> UpdateProductAsync(int id, UpdateProductDTO updateProductDto, string ImageURL)
        {
            var product = _mapper.Map<Product>(updateProductDto);
            product.ImageURL = ImageURL; // Assuming ImageURL is the path to the image
            var updatedProduct = await _productRepository.UpdateProductAsync(id, product);
            if (updatedProduct == null) return null;
            return updatedProduct;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            // Save the image to a specific path, e.g., wwwroot/images
            var product = await _productRepository.GetProductByIdAsync(id);
            try
            {

                var adminfolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
         
                var adminFilePath = Path.Combine(adminfolderPath, product.ImageURL);
       

                if (System.IO.File.Exists(adminFilePath))
                {
                    System.IO.File.Delete(adminFilePath); // Delete existing file if it exists
                }
                

            }
            catch (Exception ex)
            {
               return false; // Log the exception as needed
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

        public IQueryable<Product> GetAllProductForOData()
        {
            return _productRepository.GetAllProduct();
        }
        public IQueryable<ReadProductDTO> GetAllProduct()
        {
            return _productRepository.GetAllProduct().ProjectTo<ReadProductDTO>(_mapper.ConfigurationProvider);
        }
    }
}
