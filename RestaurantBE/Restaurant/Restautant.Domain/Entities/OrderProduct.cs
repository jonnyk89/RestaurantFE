using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Entities
{
    [Table("OrderProducts")]
    public class OrderProduct
    {
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProductPrice { get; set; }
        public int ProductQuantity { get; set; }

        [Required]
        public string ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}
