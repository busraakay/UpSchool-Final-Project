using FinalProject.Domain.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FinalProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeleniumLogsControllers : ControllerBase
    {
        private readonly IHubContext _seleniumLogHubContext;

        public SeleniumLogsControllers(IHubContext seleniumLogHubContext)
        {
            _seleniumLogHubContext = seleniumLogHubContext;
        }

        [HttpPost]
        public async Task<IActionResult> SendLogNotificationAsync(SendLogNotificationApiDto seleniumLodDto)
        {
            await _seleniumLogHubContext.Clients.AllExcept(seleniumLodDto.ConnectionId)
                .SendAsync("New Selenium Log Added", seleniumLodDto.Log);
            return Ok();
        }
    }
}
