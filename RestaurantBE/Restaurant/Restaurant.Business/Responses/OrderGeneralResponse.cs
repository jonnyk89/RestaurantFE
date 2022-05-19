using Restaurant.Domain.Entities;

namespace Restaurant.Business.Responses
{
    public class OrderGeneralResponse
    {
        public int Id { get; set; }
        public int? TableId { get; set; }
        public string Waiter { get; set; }
        public string DateTime { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public ICollection<OrderProductResponse> Products { get; set; } = new List<OrderProductResponse>();
    }
}
