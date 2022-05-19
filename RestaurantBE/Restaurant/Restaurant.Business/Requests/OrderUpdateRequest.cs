using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Requests
{
    public class OrderUpdateRequest
    {
        public string? UserId { get; set; }

        public int? TableId { get; set; }

        public ICollection<OrderProductRequest>? Products { get; set; }
    }
}
