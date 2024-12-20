using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Core.Services;
using _1likte.Data;
using _1likte.Model.DbModels;
using _1likte.Model.ViewModels.Common.MachineGo.Model.ViewModel;
using _1likte.Model.ViewModels.Product;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace _1likte.Core.Concrete
{
    public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ProductService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Create
    public async Task<ValidatedModel<Product>> CreateProductAsync(CreateProductRequestModel product)
    {
        try
        {
            // Kategoriyi kontrol et
            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category == null)
                return new ValidatedModel<Product>("Category not found.");
            
            var map = _mapper.Map<Product>(product);
            _context.Products.Add(map);
            await _context.SaveChangesAsync();

            return new ValidatedModel<Product>(map);
        }
        catch (Exception ex)
        {
            return new ValidatedModel<Product>($"An error occurred: {ex.Message}");
        }
    }

    // Get by Id
    public async Task<ValidatedModel<Product>> GetProductByIdAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (product == null)
            return new ValidatedModel<Product>("Product not found.");

        return new ValidatedModel<Product>(product);
    }

    // Get All
    public async Task<ValidatedModel<IEnumerable<Product>>> GetAllProductsAsync()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .ToListAsync();

        return new ValidatedModel<IEnumerable<Product>>(products);
    }

    // Update
    public async Task<ValidatedModel<Product>> UpdateProductAsync(UpdateProductRequestModel product)
    {
        try
        {
            var existingProduct = await _context.Products.FindAsync(product.Id);
            if (existingProduct == null)
                return new ValidatedModel<Product>("Product not found.");

            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category == null)
                return new ValidatedModel<Product>("Category not found.");

            existingProduct.Title = product.Title;
            existingProduct.Description = product.Description;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync();

            return new ValidatedModel<Product>(existingProduct);
        }
        catch (Exception ex)
        {
            return new ValidatedModel<Product>($"An error occurred: {ex.Message}");
        }
    }

    // Delete
    public async Task<ValidatedModel<bool>> DeleteProductAsync(int id)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return new ValidatedModel<bool>("Product not found.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return new ValidatedModel<bool>(true);
        }
        catch (Exception ex)
        {
            return new ValidatedModel<bool>($"An error occurred: {ex.Message}");
        }
    }
}

}