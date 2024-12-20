using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1likte.Model.ViewModels.Product
{
    public class UpdateProductRequestModel
    {
        public int Id { get; set;}
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
    }
}