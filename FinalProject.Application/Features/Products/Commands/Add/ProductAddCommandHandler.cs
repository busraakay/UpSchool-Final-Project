using FinalProject.Application.Common.Interfaces;
using FinalProject.Domain.Common;
using FinalProject.Domain.Entities;
using MediatR;

namespace FinalProject.Application.Features.Products.Commands.Add
{
    public class ProductAddCommandHandler : IRequestHandler<ProductAddCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public ProductAddCommandHandler(IApplicationDbContext applicationDbContext)
        {
                _applicationDbContext = applicationDbContext;
        }

        public async Task<Response<Guid>> Handle(ProductAddCommand request, CancellationToken cancellationToken)
        {
            var product = new Product()
            {
                OrderId = request.OrderId,
                Name = request.Name,
                Picture = request.Picture,
                IsOnSale=request.IsOnSale,
                Price = request.Price,
                SalePrice = request.SalePrice,
            };

            await _applicationDbContext.Products.AddAsync(product, cancellationToken);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Response<Guid>($"\"{product.Name}\" successfully added");

        }
    }
}
