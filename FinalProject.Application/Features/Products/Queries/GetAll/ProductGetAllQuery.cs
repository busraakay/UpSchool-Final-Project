using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Application.Features.Products.Queries.GetAll
{
    public class ProductGetAllQuery : IRequest<List<ProductGetAllDto>>
    {
        public Guid OrderId { get; set; }

        public int RequestedAmount { get; set; }

        public ProductGetAllQuery(Guid orderId, int requestedAmount)
        {
            OrderId = orderId;
            RequestedAmount = requestedAmount;

        }
    }

}
