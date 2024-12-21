using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Core.Services;
using _1likte.Model.ViewModels.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace _1likte.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        // Create Basket
        [HttpPost("create")]
        [Authorize(Roles = "Admin, User")]
        [SwaggerOperation(
            Summary = "Yeni bir alışveriş sepeti oluşturur",
            Description = "Bu işlem, kullanıcı için yeni bir alışveriş sepeti oluşturur. Sepet başarıyla oluşturulduğunda, sepetin detayları döndürülür.",
            OperationId = "CreateBasket",
            Tags = new[] { "Alışveriş Sepeti Yönetimi" }
        )]
        public async Task<IActionResult> CreateBasket([FromBody] CreateBasketRequestModel model)
        {
            var result = await _basketService.CreateBasketAsync(model);
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        // Get Basket by UserId
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin, User")]
        [SwaggerOperation(
            Summary = "Kullanıcı ID'sine göre alışveriş sepeti getirir",
            Description = "Verilen kullanıcı ID'sine sahip alışveriş sepetini getirir. Eğer kullanıcıya ait sepet bulunamazsa, 404 Not Found hatası döner.",
            OperationId = "GetBasketByUserId",
            Tags = new[] { "Alışveriş Sepeti Yönetimi" }
        )]
        public async Task<IActionResult> GetBasketByUserId(int userId)
        {
            var result = await _basketService.GetBasketByUserIdAsync(userId);
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        // Add Product to Basket
        [HttpPost("add/{userId}")]
        [Authorize(Roles = "Admin, User")]
        [SwaggerOperation(
            Summary = "Kullanıcı sepetine ürün ekler",
            Description = "Bu işlem, kullanıcı sepetine yeni bir ürün ekler. Ürün başarıyla eklendiyse, güncellenmiş sepet döndürülür.",
            OperationId = "AddProductToBasket",
            Tags = new[] { "Alışveriş Sepeti Yönetimi" }
        )]
        public async Task<IActionResult> AddProductToBasket(int userId, [FromBody] AddProductToBasketRequestModel model)
        {
            var result = await _basketService.AddProductToBasketAsync(userId, model);
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        // Remove Product from Basket
        [HttpDelete("remove/{userId}/{productId}")]
        [Authorize(Roles = "Admin, User")]
        [SwaggerOperation(
            Summary = "Kullanıcı sepetinden ürün çıkarır",
            Description = "Bu işlem, kullanıcı sepetinden belirli bir ürünü çıkarır. Ürün başarıyla çıkarıldığında, güncellenmiş sepet döndürülür.",
            OperationId = "RemoveProductFromBasket",
            Tags = new[] { "Alışveriş Sepeti Yönetimi" }
        )]
        public async Task<IActionResult> RemoveProductFromBasket(int userId, int productId)
        {
            var result = await _basketService.RemoveProductFromBasketAsync(userId, productId);
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        // Clear Basket
        [HttpDelete("clear/{userId}")]
        [Authorize(Roles = "Admin, User")]
        [SwaggerOperation(
            Summary = "Kullanıcı sepetini tamamen temizler",
            Description = "Bu işlem, kullanıcı sepetini tamamen temizler. Sepet başarıyla temizlendiyse, 204 No Content yanıtı döner.",
            OperationId = "ClearBasket",
            Tags = new[] { "Alışveriş Sepeti Yönetimi" }
        )]
        public async Task<IActionResult> ClearBasket(int userId)
        {
            var result = await _basketService.ClearBasketAsync(userId);
            if (!result.Success)
                return BadRequest(result.Error);

            return NoContent();
        }
    }
}
