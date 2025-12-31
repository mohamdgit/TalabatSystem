using ECommerce.Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Configurations.OrderConfig
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.ToTable("DeliveryMethods");
            builder.Property(D=>D.Price).HasColumnType("decimal(8,2)");
            builder.Property(D=>D.ShortName).HasColumnType("Varchar").HasMaxLength(50);
            builder.Property(D=>D.Description).HasColumnType("Varchar").HasMaxLength(100);
            builder.Property(D=>D.DeliveryTime).HasColumnType("Varchar").HasMaxLength(50);

        }
    }
}
