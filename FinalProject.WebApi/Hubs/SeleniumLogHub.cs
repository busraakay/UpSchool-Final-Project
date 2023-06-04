using FinalProject.Domain.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace FinalProject.WebApi.Hubs
{
    public class SeleniumLogHub : Hub
    {
        public async Task SendLogNotificationAsync(SeleniumLodDto lodDto) {
            //Buraya istek gönderen connectionId lı client dışında herkese mesajı gönder.
            //Blazor bizi dinlediği kanalın adı NewSeleniumLogAdded
            //Crowler biz Hub Ptt Ulaşıcağı yerde blazor
            await Clients.AllExcept(Context.ConnectionId).SendAsync("NewSeleniumLogAdded",lodDto);
        }
    }
}
