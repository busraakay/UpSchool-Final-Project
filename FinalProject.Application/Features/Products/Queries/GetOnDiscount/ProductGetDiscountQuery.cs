using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Application.Features.Products.Queries.GetOnDiscount
{
    public class ProductGetDiscountQuery : IRequest<List<ProductGetDiscountDto>>
    {
        public Guid OrderId { get; set; }

        public int RequestedAmount { get; set; }

        public ProductGetDiscountQuery(Guid orderId, int requestedAmount)
        {
            OrderId = orderId;
            RequestedAmount = requestedAmount;

        }
    }
}
