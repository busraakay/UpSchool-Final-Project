using FinalProject.Application.Features.Orders.Commands.Add;
using FinalProject.Application.Features.Orders.Commands.Update;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.WebApi.Controllers
{
    public class OrdersController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddAsync(OrderAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(OrderUpdateCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
