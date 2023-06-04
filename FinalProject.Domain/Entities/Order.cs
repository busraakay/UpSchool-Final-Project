using FinalProject.Domain.Common;
using FinalProject.Domain.Enums;

namespace FinalProject.Domain.Entities
{
    public class Order:EntityBase<Guid>
    {
      
        public int RequestedAmount { get; set; }
        public int TotalFoundedAmount { get; set; }
        public ICollection<OrderEvent> OrderEvents { get; set; }
        public ICollection<Product> Products { get; set; }
        public ProductCrowlType ProductCrowlType { get; set; }

    }
}
