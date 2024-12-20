using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1likte.Model.ViewModels.Basket
{
    public class BasketResponseModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<BasketItemResponseModel> BasketItems { get; set; }
    }
}