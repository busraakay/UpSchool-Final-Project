using FinalProject.Application.Features.OrderEvents.Commands.Add;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.WebApi.Controllers
{
    public class OrderEventsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddAsync(OrderEventAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
