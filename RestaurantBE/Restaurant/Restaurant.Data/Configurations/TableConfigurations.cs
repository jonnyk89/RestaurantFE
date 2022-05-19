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
    public class TableConfigurations : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> builder)
        {
            builder.ToTable("Tables");

            builder.HasMany(t => t.Orders)
                .WithOne(t => t.Table)
                .HasForeignKey(x => x.TableId);


            builder.HasOne(t => t.Waiter)
                .WithMany(w => w.Tables)
                .HasForeignKey(t => t.WaiterId);
        }
    }
}
