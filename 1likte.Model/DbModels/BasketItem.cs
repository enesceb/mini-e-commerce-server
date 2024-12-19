using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1likte.Model.DbModels
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}