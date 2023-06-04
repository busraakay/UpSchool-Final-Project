    using FinalProject.Application.Common.Interfaces;
using FinalProject.Application.Common.Models.Email;
using FinalProject.Application.Features.Products.Commands.Add;
using FinalProject.Domain.Common;
using FinalProject.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Application.Features.OrderEvents.Commands.Add
{
    public class OrderEventAddCommandHandler : IRequestHandler<OrderEventAddCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        //private readonly IEmailService _emailService;

        public OrderEventAddCommandHandler(IApplicationDbContext applicationDbContext/*, IEmailService emailService*/)
        {
            _applicationDbContext = applicationDbContext;
            //_emailService = emailService;
        }

        public async Task<Response<Guid>> Handle(OrderEventAddCommand request, CancellationToken cancellationToken)
        {
            var orderEvent = new OrderEvent()
            {
                OrderId = request.OrderId,
                Status = request.Status,
            };

            //_emailService.SendEmailStatus(new SendEmailStatusDto()
            //{
            //    Email = "example@email.com",
            //    Message = request.Status.ToString()
            //});


            await _applicationDbContext.OrderEvents.AddAsync(orderEvent, cancellationToken);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Response<Guid>($"OrderEvent successfully added.");

        }
    }
}
