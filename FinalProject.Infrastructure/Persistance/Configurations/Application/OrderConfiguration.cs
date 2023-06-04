using FinalProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinalProject.Infrastructure.Persistance.Configurations.Application
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            //Id
            builder.HasKey(x => x.Id);

            //Properties
            builder.Property(x=>x.RequestedAmount).IsRequired();
            builder.Property(x=>x.TotalFoundedAmount).IsRequired();
            builder.Property(x=>x.ProductCrowlType).IsRequired();
            builder.Property(x => x.ProductCrowlType).HasConversion<int>();



            //Relationships
            builder.HasMany<Product>(x => x.Products).WithOne(x => x.Order).HasForeignKey(x => x.OrderId);
            builder.HasMany<OrderEvent>(x => x.OrderEvents).WithOne(x => x.Order).HasForeignKey(x => x.OrderId);

            builder.ToTable("Orders");

        }
    }
}
