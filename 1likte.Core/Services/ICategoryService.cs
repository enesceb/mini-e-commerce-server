using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Model.DbModels;
using _1likte.Model.ViewModels.Common.MachineGo.Model.ViewModel;

namespace _1likte.Core.Services
{
    public interface ICategoryService
    {
        Task<ValidatedModel<Category>> CreateCategoryAsync(Category category);
        Task<ValidatedModel<Category>> GetCategoryByIdAsync(int id);
        Task<ValidatedModel<IEnumerable<Category>>> GetAllCategoriesAsync();
        Task<ValidatedModel<Category>> UpdateCategoryAsync(Category category);
        Task<ValidatedModel<bool>> DeleteCategoryAsync(int id);
    }
}