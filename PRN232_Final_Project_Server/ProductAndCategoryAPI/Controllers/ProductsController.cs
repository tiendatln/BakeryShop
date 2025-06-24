using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using ProductAndCategoryAPI.Data;
using ProductAndCategoryAPI.DTOs;
using ProductAndCategoryAPI.Models;
using ProductAndCategoryAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAndCategoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _product;

        public ProductsController(IProductService product)
        {
            _product = product;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadProductDTO>>> GetProducts()
        {
            var products = await _product.GetAllProductsAsync();
            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadProductDTO>> GetProduct(int id)
        {
            var product = await _product.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, UpdateProductDTO product)
        {
            

            // Nếu cập nhật mà không có ảnh mới, thì giữ nguyên ảnh cũ
            if (string.IsNullOrWhiteSpace(product.ImageURL))
            {
                var existing = await _product.GetProductByIdAsync(id);
                if (existing == null)
                {
                    return NotFound();
                }

                product.ImageURL = existing.ImageURL;
            }

            var updatedProduct = await _product.UpdateProductAsync(id, product);
            return Ok(updatedProduct);
        }


        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CreateProductDTO>> PostProduct(CreateProductDTO product)
        {
            if (product == null)
            {

                return BadRequest("Product data is null or image is not provided.");
            }

            var createdProduct = await _product.CreateProductAsync(product);
            return Ok(createdProduct);
        }


        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _product.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _product.DeleteProductAsync(id);

            return Ok(true);
        }

        private async Task<bool> ProductExists(int id)
        {
            return await _product.GetProductByIdAsync(id) != null;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ReadProductDTO>>> SearchProduct([FromQuery] string searchKey)
        {
            if (string.IsNullOrWhiteSpace(searchKey))
            {
                return BadRequest("Search term cannot be empty.");
            }
            var products = await _product.SearchProductsAsync(searchKey);
            if (products == null || !products.Any())
            {
                return NotFound("No products found matching the search term.");
            }
            return Ok(products);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ReadProductDTO>>> GetProductsByCategoryId(int categoryId)
        {
            var products = await _product.GetProductsByCategoryIdAsync(categoryId);
            if (products == null || !products.Any())
            {
                return NotFound("No products found for the specified category.");
            }
            return Ok(products);
        }

        // GET: api/Products/Get
        [HttpGet("Get")]
        [EnableQuery]
        public IQueryable<Product> GetAvailableProducts()
        {
            return _product.GetAvailableProductsAsync();

        }
    }
}
