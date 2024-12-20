using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Core.Services;
using _1likte.Data;
using _1likte.Model.DbModels;
using _1likte.Model.ViewModels.Common.MachineGo.Model.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace _1likte.Core.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create
        public async Task<ValidatedModel<Category>> CreateCategoryAsync(Category category)
        {
            try
            {
                if (_context.Categories.Any(c => c.Name == category.Name))
                    return new ValidatedModel<Category>("Category with this name already exists.");

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return new ValidatedModel<Category>(category);
            }
            catch (Exception ex)
            {
                return new ValidatedModel<Category>($"An error occurred: {ex.Message}");
            }
        }

        // Get by Id
        public async Task<ValidatedModel<Category>> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return new ValidatedModel<Category>("Category not found.");

            return new ValidatedModel<Category>(category);
        }

        // Get All
        public async Task<ValidatedModel<IEnumerable<Category>>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return new ValidatedModel<IEnumerable<Category>>(categories);
        }

        // Update
        public async Task<ValidatedModel<Category>> UpdateCategoryAsync(Category category)
        {
            try
            {
                var existingCategory = await _context.Categories.FindAsync(category.Id);
                if (existingCategory == null)
                    return new ValidatedModel<Category>("Category not found.");

                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;

                await _context.SaveChangesAsync();

                return new ValidatedModel<Category>(existingCategory);
            }
            catch (Exception ex)
            {
                return new ValidatedModel<Category>($"An error occurred: {ex.Message}");
            }
        }

        // Delete
        public async Task<ValidatedModel<bool>> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                    return new ValidatedModel<bool>("Category not found.");

                _context.Categories.Remove(category);
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