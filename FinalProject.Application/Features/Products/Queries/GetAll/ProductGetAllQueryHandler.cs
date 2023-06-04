using FinalProject.Application.Common.Interfaces;
using FinalProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Application.Features.Products.Queries.GetAll
{
    public class ProductGetAllQueryHandler : IRequestHandler<ProductGetAllQuery, List<ProductGetAllDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public ProductGetAllQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<ProductGetAllDto>> Handle(ProductGetAllQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.Products.AsQueryable();

            dbQuery = dbQuery.Where(x => x.OrderId == request.OrderId).Take(request.RequestedAmount);

            var products = await dbQuery.ToListAsync(cancellationToken);

            var productDtos = MapProductsToGetAllDtos(products);

            return productDtos.ToList();
        }

        private IEnumerable<ProductGetAllDto> MapProductsToGetAllDtos(List<Product> products)
        {
            foreach (var product in products)
            {
                yield return new ProductGetAllDto()
                {
                    Name = product.Name,
                    Picture = product.Picture,
                    IsOnSale = product.IsOnSale,
                    Price = product.Price,
                    SalePrice = product.SalePrice,
                };
            }
        }
    }

}
