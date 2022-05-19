using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Domain.Entities
{
    [Table("Categories")]
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string? Name { get; set; }
        public string? ParentId { get; set; } = null;

        public virtual Category? Parent { get; set; }
        public virtual ICollection<Category>? Children { get; set; }
        public virtual ICollection<Product>? Products { get; set; } = new List<Product>();
    }
}
