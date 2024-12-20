using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Model.ViewModels.Basket;
using _1likte.Model.ViewModels.Common.MachineGo.Model.ViewModel;

namespace _1likte.Core.Services
{
    public interface IBasketService
    {
        Task<ValidatedModel<BasketResponseModel>> CreateBasketAsync(CreateBasketRequestModel model);
        Task<ValidatedModel<BasketResponseModel>> GetBasketByUserIdAsync(int userId);
        Task<ValidatedModel<BasketResponseModel>> AddProductToBasketAsync(int userId, AddProductToBasketRequestModel model);
        Task<ValidatedModel<BasketResponseModel>> RemoveProductFromBasketAsync(int userId, int productId);
        Task<ValidatedModel<bool>> ClearBasketAsync(int userId);
    }
}