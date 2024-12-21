using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Core.Services;
using _1likte.Model.DbModels;
using _1likte.Model.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(
            Summary = "Yeni bir ürün oluşturur",
            Description = "Bu işlem, verilen ürün detaylarıyla yeni bir ürün oluşturur. Başarıyla oluşturulan ürünün bilgileri döndürülür.",
            OperationId = "CreateProduct",
            Tags = new[] { "Ürün Yönetimi" }
        )]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestModel product)
        {
            var result = await _productService.CreateProductAsync(product);
            if (!result.Success)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetProductById), new { id = result.Data.Id }, result.Data);
        }

        // Get Product by Id
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "ID'ye göre ürünü getirir",
            Description = "Bu işlem, verilen ürün ID'sine göre ürünü getirir. Eğer ürün bulunamazsa, 404 Not Found hatası döner.",
            OperationId = "GetProductById",
            Tags = new[] { "Ürün Yönetimi" }
        )]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        // Get All Products
        [HttpGet]
        [SwaggerOperation(
            Summary = "Tüm ürünleri getirir",
            Description = "Bu işlem, tüm ürünlerin listesini getirir. Eğer ürün bulunmazsa, 404 Not Found hatası dönebilir.",
            OperationId = "GetAllProducts",
            Tags = new[] { "Ürün Yönetimi" }
        )]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProductsAsync();
            return Ok(result.Data);
        }

        // Update Product
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Ürünü günceller",
            Description = "Bu işlem, verilen ürün ID'sine göre ürünü günceller. Eğer ID uyuşmazsa, 400 Bad Request hatası döner.",
            OperationId = "UpdateProduct",
            Tags = new[] { "Ürün Yönetimi" }
        )]
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
        [SwaggerOperation(
            Summary = "Ürünü siler",
            Description = "Bu işlem, verilen ürün ID'sine sahip ürünü siler. Başarıyla silindiyse, 204 No Content döner.",
            OperationId = "DeleteProduct",
            Tags = new[] { "Ürün Yönetimi" }
        )]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result.Success)
                return BadRequest(result.Error);

            return NoContent(); // Successfully deleted
        }
    }
}
