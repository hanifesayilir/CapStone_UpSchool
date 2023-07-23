using Application.Common.Models.Crawler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs
{
    public class CrawlerLogHub:Hub
    {

        [Authorize]
        public async Task SendLogNotificationAsync(CrawlerLogDto log)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("NewCrawlerLogAdded", log);
        }
 
    }
}
