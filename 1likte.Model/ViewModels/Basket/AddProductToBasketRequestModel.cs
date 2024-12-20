using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1likte.Model.ViewModels.Basket
{
    public class AddProductToBasketRequestModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}