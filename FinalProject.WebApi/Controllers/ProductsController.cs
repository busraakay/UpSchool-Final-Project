using FinalProject.Application.Features.Products.Commands.Add;
using FinalProject.Application.Features.Products.Queries.GetAll;
using FinalProject.Application.Features.Products.Queries.GetNonDiscount;
using FinalProject.Application.Features.Products.Queries.GetOnDiscount;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.WebApi.Controllers
{
    public class ProductsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddAsync(ProductAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllAsync(ProductGetAllQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpPost("GetNonDiscount")]
        public async Task<IActionResult> GetAllAsync(ProductGetNonDiscountQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpPost("GetDiscount")]
        public async Task<IActionResult> GetAllAsync(ProductGetDiscountQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
