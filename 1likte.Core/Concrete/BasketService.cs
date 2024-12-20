using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Core.Services;
using _1likte.Data;
using _1likte.Model.DbModels;
using _1likte.Model.ViewModels.Basket;
using _1likte.Model.ViewModels.Common.MachineGo.Model.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace _1likte.Core.Concrete
{
    public class BasketService : IBasketService
    {
        private readonly ApplicationDbContext _context;

        public BasketService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create Basket
        public async Task<ValidatedModel<BasketResponseModel>> CreateBasketAsync(CreateBasketRequestModel model)
        {
            var existingBasket = await _context.Baskets.FirstOrDefaultAsync(b => b.UserId == model.UserId);
            if (existingBasket != null)
                return new ValidatedModel<BasketResponseModel>("Basket already exists.");

            var basket = new Basket { UserId = model.UserId, Items = new List<BasketItem>() };
            _context.Baskets.Add(basket);
            await _context.SaveChangesAsync();

            var response = new BasketResponseModel
            {
                Id = basket.Id,
                UserId = basket.UserId,
                BasketItems = new List<BasketItemResponseModel>()
            };

            return new ValidatedModel<BasketResponseModel>(response);
        }

        // Get Basket by UserId
        public async Task<ValidatedModel<BasketResponseModel>> GetBasketByUserIdAsync(int userId)
        {
            var basket = await _context.Baskets
                .Include(b => b.Items)
                .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null)
                return new ValidatedModel<BasketResponseModel>("Basket not found.");

            var response = new BasketResponseModel
            {
                Id = basket.Id,
                UserId = basket.UserId,
                BasketItems = basket.Items.Select(bi => new BasketItemResponseModel
                {
                    ProductId = bi.ProductId,
                    ProductName = bi.Product.Title,
                    ProductImage = bi.Product.ImageUrl,
                    Quantity = bi.Quantity,
                    Price = bi.Product.Price
                }).ToList()
            };

            return new ValidatedModel<BasketResponseModel>(response);
        }

        // Add Product to Basket
        public async Task<ValidatedModel<BasketResponseModel>> AddProductToBasketAsync(int userId, AddProductToBasketRequestModel model)
        {
            var basket = await _context.Baskets.Include(b => b.Items).FirstOrDefaultAsync(b => b.UserId == userId);
            if (basket == null)
                 basket = new Basket { UserId = userId, Items = new List<BasketItem>() };

            var product = await _context.Products.FindAsync(model.ProductId);
            if (product == null)
                return new ValidatedModel<BasketResponseModel>("Product not found.");

            var basketItem = basket.Items.FirstOrDefault(bi => bi.ProductId == model.ProductId);
            if (basketItem != null)
            {
                basketItem.Quantity += model.Quantity;
            }
            else
            {
                basket.Items.Add(new BasketItem
                {
                    ProductId = model.ProductId,
                    Quantity = model.Quantity
                });
            }

            await _context.SaveChangesAsync();

            return await GetBasketByUserIdAsync(userId); // Güncel sepeti döndürüyoruz.
        }

        // Remove Product from Basket
        public async Task<ValidatedModel<BasketResponseModel>> RemoveProductFromBasketAsync(int userId, int productId)
        {
            var basket = await _context.Baskets.Include(b => b.Items).FirstOrDefaultAsync(b => b.UserId == userId);
            if (basket == null)
                return new ValidatedModel<BasketResponseModel>("Basket not found.");

            var basketItem = basket.Items.FirstOrDefault(bi => bi.ProductId == productId);
            if (basketItem == null)
                return new ValidatedModel<BasketResponseModel>("Product not found in the basket.");

            basket.Items.Remove(basketItem);
            await _context.SaveChangesAsync();

            return await GetBasketByUserIdAsync(userId);
        }

        // Clear Basket
        public async Task<ValidatedModel<bool>> ClearBasketAsync(int userId)
        {
            var basket = await _context.Baskets.Include(b => b.Items).FirstOrDefaultAsync(b => b.UserId == userId);
            if (basket == null)
                return new ValidatedModel<bool>("Basket not found.");

            _context.BasketItems.RemoveRange(basket.Items);
            await _context.SaveChangesAsync();

            return new ValidatedModel<bool>(true);
        }
    }
}