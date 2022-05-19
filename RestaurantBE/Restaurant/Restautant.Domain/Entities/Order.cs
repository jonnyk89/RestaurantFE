using Restaurant.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Entities
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Active;
        public DateTime Date { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int TableId { get; set; }
        [ForeignKey("TableId")]
        public Table Table { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}
