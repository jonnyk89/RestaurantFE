
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Responses
{
    public class OrderTableResponse
    {
        public string Id { get; set; }
        public string WaiterId { get; set; }
        public string Waiter { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public ICollection<OrderGeneralResponse> Products { get; set; } = new List<OrderGeneralResponse>();
    }
}
