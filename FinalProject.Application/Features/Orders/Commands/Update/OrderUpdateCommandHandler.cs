using FinalProject.Application.Common.Interfaces;
using FinalProject.Application.Features.Orders.Commands.Add;
using FinalProject.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Application.Features.Orders.Commands.Update
{
    public class OrderUpdateCommandHandler : IRequestHandler<OrderUpdateCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public OrderUpdateCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Response<Guid>> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
        {
            var entity = await _applicationDbContext.Orders
            .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new Exception();
            }

            entity.RequestedAmount = request.RequestedAmount;
            entity.TotalFoundedAmount = request.TotalFoundedAmount;
            entity.ProductCrowlType = request.ProductCrowlType;

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return new Response<Guid>($"The order was successfully updated.", entity.Id);
        }
    }
}
