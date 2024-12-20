using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Core.Services;
using _1likte.Model.ViewModels.Basket;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("create")]
        public async Task<IActionResult> CreateBasket([FromBody] CreateBasketRequestModel model)
        {
            var result = await _basketService.CreateBasketAsync(model);
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBasketByUserId(int userId)
        {
            var result = await _basketService.GetBasketByUserIdAsync(userId);
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        [HttpPost("add/{userId}")]
        public async Task<IActionResult> AddProductToBasket(int userId, [FromBody] AddProductToBasketRequestModel model)
        {
            var result = await _basketService.AddProductToBasketAsync(userId, model);
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        [HttpDelete("remove/{userId}/{productId}")]
        public async Task<IActionResult> RemoveProductFromBasket(int userId, int productId)
        {
            var result = await _basketService.RemoveProductFromBasketAsync(userId, productId);
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearBasket(int userId)
        {
            var result = await _basketService.ClearBasketAsync(userId);
            if (!result.Success)
                return BadRequest(result.Error);

            return NoContent();
        }
    }

}