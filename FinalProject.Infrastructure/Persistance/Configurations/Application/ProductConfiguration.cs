using FinalProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinalProject.Infrastructure.Persistance.Configurations.Application
{

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            //Id
            builder.HasKey(x => x.Id);

            //Properties
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.IsOnSale).IsRequired();
            builder.Property(x=>x.Picture).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.SalePrice).IsRequired(false);


            builder.ToTable("Products");


        }
    }
}
