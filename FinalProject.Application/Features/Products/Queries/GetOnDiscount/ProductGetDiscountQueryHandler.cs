using FinalProject.Application.Common.Interfaces;
using FinalProject.Application.Features.Products.Queries.GetNonDiscount;
using FinalProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Application.Features.Products.Queries.GetOnDiscount
{
    public class ProductGetDiscountQueryHandler : IRequestHandler<ProductGetDiscountQuery, List<ProductGetDiscountDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public ProductGetDiscountQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<ProductGetDiscountDto>> Handle(ProductGetDiscountQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.Products.AsQueryable();

            dbQuery = dbQuery.Where(x => x.OrderId == request.OrderId && x.IsOnSale == true).Take(request.RequestedAmount);

            var products = await dbQuery.ToListAsync(cancellationToken);

            var productDtos = MapProductsToGetDiscountDtos(products);

            return productDtos.ToList();
        }

        private IEnumerable<ProductGetDiscountDto> MapProductsToGetDiscountDtos(List<Product> products)
        {
            foreach (var product in products)
            {
                yield return new ProductGetDiscountDto()
                {
                    Name = product.Name,
                    Picture = product.Picture,
                    Price = product.Price,
                    SalePrice = product.SalePrice,
                };
            }
        }
    }
}
