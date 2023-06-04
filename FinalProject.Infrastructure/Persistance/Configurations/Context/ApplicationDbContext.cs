using FinalProject.Application.Common.Interfaces;
using FinalProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FinalProject.Infrastructure.Persistance.Configurations.Context
{
    public class ApplicationDbContext:DbContext,IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<OrderEvent> OrderEvents { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
      
    }
}
