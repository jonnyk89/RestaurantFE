using Restaurant.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Domain.Entities
{
    [Table("Tables")]
    public class Table
    {
        public int Id { get; set; }
        public TableStatus Status { get; set; } = TableStatus.Free;
        public int Capacity { get; set; }
        public string? WaiterId { get; set; }
        public User? Waiter { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
}
