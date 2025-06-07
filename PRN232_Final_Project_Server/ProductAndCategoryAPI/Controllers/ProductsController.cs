using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
            if (id != product.ProductID)
            {
                return BadRequest();
            }

            await _product.UpdateProductAsync(id, product);

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CreateProductDTO>> PostProduct(CreateProductDTO product, IFormFile imgFile)
        {
            string imageUrl = "";
            if (imgFile != null && imgFile.Length > 0)
            {
                // Ví dụ lưu ảnh vào thư mục wwwroot/images
                var fileName = Path.GetFileName(imgFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "/img", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imgFile.CopyToAsync(stream);
                }

                // Gắn đường dẫn ảnh vào DTO nếu cần
                product.ImageURL = $"/img/{fileName}";
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

            return NoContent();
        }

        private async Task<bool> ProductExists(int id)
        {
            return await _product.GetProductByIdAsync(id) != null;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ReadProductDTO>>> SearchProduct(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term cannot be empty.");
            }
            var products = await _product.SearchProductsAsync(searchTerm);
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

        [HttpGet("page/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<IEnumerable<ReadProductDTO>>> GetPage(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }
            var products = await _product.GetPageAsync(pageNumber, pageSize);
            if (products == null || !products.Any())
            {
                return NotFound("No products found on this page.");
            }
            return Ok(products);
        }
    }
}
