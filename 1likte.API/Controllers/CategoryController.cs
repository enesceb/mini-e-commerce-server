using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Core.Services;
using _1likte.Model.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.Annotations;

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
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Yeni bir kategori oluşturur",
            Description = "Bu işlem, Admin rolüne sahip bir kullanıcı tarafından yapılabilir. Kategori verisi, istek gövdesinde sağlanan kategori bilgileri ile oluşturulur. Başarılı bir işlemde, oluşturulan kategorinin bilgileri ve oluşturulma zamanı geri döndürülür.",
            OperationId = "CreateCategory",
            Tags = new[] { "Kategori Yönetimi" }
        )]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            var result = await _categoryService.CreateCategoryAsync(category);
            if (!result.Success)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetCategoryById), new { id = result.Data.Id }, result.Data);
        }

        // Get Category by Id
        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Kategori ID'sine göre bir kategori getirir",
            Description = "Verilen kategori ID'sine sahip kategori bilgilerini getirir. Eğer kategori bulunamazsa, 404 Not Found hatası döner.",
            OperationId = "GetCategoryById",
            Tags = new[] { "Kategori Yönetimi" }
        )]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        // Get All Categories
        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Tüm kategorileri listeler",
            Description = "Bu işlem, tüm kategorilerin bir listesini döndürür. Herhangi bir kimlik doğrulama gerektirmez ve anonim kullanıcılar tarafından erişilebilir.",
            OperationId = "GetAllCategories",
            Tags = new[] { "Kategori Yönetimi" }
        )]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            return Ok(result.Data);
        }

        // Update Category
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Mevcut bir kategoriyi günceller",
            Description = "Bu işlem, Admin rolüne sahip bir kullanıcı tarafından yapılabilir. Kategori ID'si ile birlikte güncellenmiş kategori verileri sağlanmalıdır. Kategori ID'si sağlanan kategori verisi ile uyumsuzsa, 400 Bad Request hatası döner.",
            OperationId = "UpdateCategory",
            Tags = new[] { "Kategori Yönetimi" }
        )]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (id != category.Id)
                return BadRequest("Kategori ID uyuşmazlığı.");

            var result = await _categoryService.UpdateCategoryAsync(category);
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        // Delete Category
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Bir kategoriyi siler",
            Description = "Bu işlem, Admin rolüne sahip bir kullanıcı tarafından yapılabilir. Silme işlemi başarılı olursa, hiçbir içerik döndürülmeden 204 No Content yanıtı verilir. Eğer kategori bulunamazsa, 400 Bad Request hatası döner.",
            OperationId = "DeleteCategory",
            Tags = new[] { "Kategori Yönetimi" }
        )]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result.Success)
                return BadRequest(result.Error);

            return NoContent(); // Başarıyla silindi
        }
    }
}
