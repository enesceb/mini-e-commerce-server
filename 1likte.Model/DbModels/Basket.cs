using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1likte.Model.DbModels
{
    public class Basket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}