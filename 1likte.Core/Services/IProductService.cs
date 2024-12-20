using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Model.DbModels;
using _1likte.Model.ViewModels.Common.MachineGo.Model.ViewModel;
using _1likte.Model.ViewModels.Product;

namespace _1likte.Core.Services
{
    public interface IProductService
    {
        Task<ValidatedModel<Product>> CreateProductAsync(CreateProductRequestModel product);
        Task<ValidatedModel<Product>> GetProductByIdAsync(int id);
        Task<ValidatedModel<IEnumerable<Product>>> GetAllProductsAsync();
        Task<ValidatedModel<Product>> UpdateProductAsync(UpdateProductRequestModel product);
        Task<ValidatedModel<bool>> DeleteProductAsync(int id);
    }
}