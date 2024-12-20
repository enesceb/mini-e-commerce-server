using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Core.Services;
using _1likte.Model.DbModels;
using _1likte.Model.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace _1likte.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // Create Product
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestModel product)
        {
            var result = await _productService.CreateProductAsync(product);
            if (!result.Success)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetProductById), new { id = result.Data.Id }, result.Data);
        }

        // Get Product by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        // Get All Products
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProductsAsync();
            return Ok(result.Data);
        }

        // Update Product
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequestModel product)
        {
            if (id != product.Id)
                return BadRequest("Product ID mismatch.");

            var result = await _productService.UpdateProductAsync(product);
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        // Delete Product
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result.Success)
                return BadRequest(result.Error);

            return NoContent(); // Successfully deleted
        }
    }

}