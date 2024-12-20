using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Core.Services;
using _1likte.Model.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _1likte.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Create Category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            var result = await _categoryService.CreateCategoryAsync(category);
            if (!result.Success)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetCategoryById), new { id = result.Data.Id }, result.Data);
        }

        // Get Category by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        // Get All Categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            return Ok(result.Data);
        }

        // Update Category
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (id != category.Id)
                return BadRequest("Category ID mismatch.");

            var result = await _categoryService.UpdateCategoryAsync(category);
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        // Delete Category
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result.Success)
                return BadRequest(result.Error);

            return NoContent(); // Successfully deleted
        }
    }

}