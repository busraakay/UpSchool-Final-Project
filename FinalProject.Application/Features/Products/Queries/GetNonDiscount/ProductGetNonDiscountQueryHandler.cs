using FinalProject.Application.Common.Interfaces;
using FinalProject.Application.Features.Products.Queries.GetAll;
using FinalProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Application.Features.Products.Queries.GetNonDiscount
{
    public class ProductGetNonDiscountQueryHandler : IRequestHandler<ProductGetNonDiscountQuery, List<ProductGetNonDiscountDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public ProductGetNonDiscountQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<ProductGetNonDiscountDto>> Handle(ProductGetNonDiscountQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.Products.AsQueryable();

            dbQuery = dbQuery.Where(x => x.OrderId == request.OrderId && x.IsOnSale == false).Take(request.RequestedAmount);

            var products = await dbQuery.ToListAsync(cancellationToken);

            var productDtos = MapProductsToGetNonDiscountDtos(products);

            return productDtos.ToList();
        }

        private IEnumerable<ProductGetNonDiscountDto> MapProductsToGetNonDiscountDtos(List<Product> products)
        {
            foreach (var product in products)
            {
                yield return new ProductGetNonDiscountDto()
                {
                    Name = product.Name,
                    Picture = product.Picture,
                    Price = product.Price,
                };
            }
        }
    }
}
