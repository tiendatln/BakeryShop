using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ProductAndCategoryAPI.DTOs;
using ProductAndCategoryAPI.Models;
using ProductAndCategoryAPI.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAndCategoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _category;

        public CategoriesController(ICategoryService category)
        {
            _category = category;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadCategoryDTO>>> GetCategories()
        {
            var categories = await _category.GetAllCategoriesAsync();
            if (categories == null || !categories.Any())
            {
                return NotFound("No categories found.");
            }
            return Ok(categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadCategoryDTO>> GetCategory([FromQuery] int id)
        {
            var category = await _category.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound("Category not found.");
            }
            return Ok(category);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, [FromBody] UpdateCategoryDTO category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.CategoryName))
            {
                return BadRequest("Invalid category data.");
            }

            var updatedCategory = await _category.UpdateCategoryAsync(id, category);
            if (updatedCategory == null)
            {
                return NotFound("Category not found.");
            }

            return Ok(updatedCategory);
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<ReadCategoryDTO>> PostCategory([FromBody] CreateCategoryDTO category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.CategoryName))
            {
                return BadRequest("Invalid category data.");
            }

            var createdCategory = await _category.CreateCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.CategoryID }, createdCategory);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _category.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound("Category not found.");
            }

            var result = await _category.DeleteCategoryAsync(id);
            if (!result)
            {
                return BadRequest("Cannot delete category. It may have associated products.");
            }

            return Ok(true);
        }

        // GET: api/Categories/search?searchTerm=abc
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ReadCategoryDTO>>> SearchCategory([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term cannot be empty.");
            }

            var categories = await _category.SearchCategoriesAsync(searchTerm);
            if (categories == null || !categories.Any())
            {
                return NotFound("No categories found matching the search term.");
            }

            return Ok(categories);
        }


        // GET: api/Categories/Get
        [HttpGet("Get")]
        [EnableQuery]
        public IQueryable<Category> GetAvailableCategories()
        {
            return _category.GetAvailableCategoriesAsync();
        }

        private async Task<bool> CategoryExists(int id)
        {
            return await _category.GetCategoryByIdAsync(id) != null;
        }
    }
}
