using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Responses
{
    public class ProductResponse
    {
        public string productId { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? CategoryId { get; set; }

        public string? Category { get; set; }

        public decimal Price { get; set; }

        public ProductResponse(Product product)
        {
            productId = product.Id;
            Name = product.Name;
            Description = product.Description;
            CategoryId = product.CategoryId;
            Category = product.Category.Name;
            Price = product.Price;
        }
    }
}
