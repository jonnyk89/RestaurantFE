
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int? TableId { get; set; }
        public string Waiter { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public ICollection<OrderProductResponse> Products { get; set; } = new List<OrderProductResponse>();
    }
}
