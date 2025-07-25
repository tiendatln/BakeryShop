﻿using Microsoft.AspNetCore.Http;
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
        private readonly IWebHostEnvironment _env;

        public ProductsController(IProductService product, IWebHostEnvironment env)
        {
            _product = product;
            _env = env;
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] UpdateProductDTO product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Khớp ID
            if (id != product.ProductID)
            {
                ModelState.AddModelError("", "ProductID mismatch.");
                return BadRequest(ModelState);
            }

            var existingProduct = await _product.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                ModelState.AddModelError("", "Product not found.");
                return BadRequest(ModelState);
            }

            var uniqueFileName = string.Empty;
            var newImageURL = string.Empty;

            if (product.ImageURL != null && product.ImageURL.Length > 0)
            {
                var adminfolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img/product");
                uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(product.ImageURL.FileName);
                var adminFilePath = Path.Combine(adminfolderPath, uniqueFileName);
                newImageURL = $"img/product/{uniqueFileName}";
                Directory.CreateDirectory(adminfolderPath);

                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingProduct.ImageURL);
                if (System.IO.File.Exists(oldFilePath) && existingProduct.ImageURL != newImageURL)
                {
                    System.IO.File.Delete(oldFilePath);
                }

                using (var stream = new FileStream(adminFilePath, FileMode.Create))
                {
                    await product.ImageURL.CopyToAsync(stream);
                }
            }
            else
            {
                uniqueFileName = existingProduct.ImageURL;
                newImageURL = uniqueFileName; // giữ lại URL cũ
            }
            Console.WriteLine($"❗ Đang cố cập nhật sản phẩm với ID = {id} và ImageURL = {newImageURL}");
            Console.WriteLine($"❗ Đang cố cập nhật sản phẩm với Product: {product.ToString()}");

            var updatedProduct = await _product.UpdateProductAsync(id, product, newImageURL);
            return Ok(updatedProduct);
        }




        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ReadProductDTO>> PostProduct([FromForm] CreateProductDTO product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (product.ImageURL == null || product.ImageURL.Length == 0)
                return BadRequest("Image file is required.");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "img/product");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(product.ImageURL.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await product.ImageURL.CopyToAsync(stream);
            }

            var createdProduct = await _product.CreateProductAsync(product, $"img/product/{uniqueFileName}");
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
    }
}
