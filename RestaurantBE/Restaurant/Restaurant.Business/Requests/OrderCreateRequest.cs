using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.MessageResponses;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Business.Requests
{
    public class OrderCreateRequest
    {
        [Required(ErrorMessage = Messages.OrderTableIdRequired)]
        public int TableId { get; set; }

        public ICollection<OrderProductRequest> Products { get; set; }
    }
}
