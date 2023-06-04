using FinalProject.Domain.Common;
using FinalProject.Domain.Entities;
using FinalProject.Domain.Enums;

namespace FinalProject.Domain.Entities
{
    public class OrderEvent: EntityBase<Guid>
    {
        public Guid OrderId { get; set; }

        public Order Order { get; set; }

        public OrderStatus Status { get; set; }
    }
}
