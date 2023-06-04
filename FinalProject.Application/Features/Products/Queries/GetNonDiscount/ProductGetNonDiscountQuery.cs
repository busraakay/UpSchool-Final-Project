using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Application.Features.Products.Queries.GetNonDiscount
{
    public class ProductGetNonDiscountQuery : IRequest<List<ProductGetNonDiscountDto>>
    {
        public Guid OrderId { get; set; }

        public int RequestedAmount { get; set; }

        public ProductGetNonDiscountQuery(Guid orderId, int requestedAmount)
        {
            OrderId = orderId;
            RequestedAmount = requestedAmount;

        }
    }
}
