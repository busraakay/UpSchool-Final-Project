using FinalProject.Application.Features.Products.Commands.Add;
using FinalProject.Domain.Common;
using FinalProject.Domain.Entities;
using FinalProject.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Application.Features.Orders.Commands.Add
{
    public class OrderAddCommand : IRequest<Response<Guid>>
    {
        public Guid Id { get; set; }
        public int RequestedAmount { get; set; }
        public int TotalFoundedAmount { get; set; }
        public ProductCrowlType ProductCrowlType { get; set; }
    }
}
