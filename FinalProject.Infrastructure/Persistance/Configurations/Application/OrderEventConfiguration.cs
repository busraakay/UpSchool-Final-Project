using FinalProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinalProject.Infrastructure.Persistance.Configurations.Application
{
    public class OrderEventConfiguration : IEntityTypeConfiguration<OrderEvent>
    {
        public void Configure(EntityTypeBuilder<OrderEvent> builder)
        {
            //Id
            builder.HasKey(e => e.Id);


            //
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Status).HasConversion<int>();


            builder.ToTable("OrderEvents");

        }
    }
}
