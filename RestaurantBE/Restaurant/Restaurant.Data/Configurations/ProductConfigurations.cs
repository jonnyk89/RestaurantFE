using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(x => x.CategoryId);

            builder.HasMany(p => p.OrderProducts)
                .WithOne(op => op.Product)
                .HasForeignKey(op => op.ProductId);
        }
    }
}
