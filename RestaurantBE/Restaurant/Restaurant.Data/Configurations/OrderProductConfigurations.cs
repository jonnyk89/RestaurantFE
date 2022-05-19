using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;

namespace Restaurant.Data.Configurations
{
    public class OrderProductConfigurations : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.ToTable("OrderProducts");

            builder.HasKey(op => new { op.OrderId, op.ProductId });

            builder.HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId);

            builder.HasOne(op => op.Product)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.ProductId);
        }
    }
}
