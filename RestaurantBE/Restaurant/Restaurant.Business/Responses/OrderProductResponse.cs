namespace Restaurant.Business.Responses
{
    public class OrderProductResponse
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
