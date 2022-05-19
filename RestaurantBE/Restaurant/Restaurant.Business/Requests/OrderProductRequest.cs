using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Requests
{
    public class OrderProductRequest
    {
        public string productId { get; set; }

        public int quantity { get; set; }

        public OrderProduct ToOrderProductModel()
            => new OrderProduct
            {
                ProductId = productId,
                ProductQuantity = quantity,
            };
        
    }
}
