using Application.Common.Models.Crawler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs
{
   
    public class CrawlerSendingParameters:Hub
    {
        
        public async Task SendParameters(CrawlingParametersDto crawlingParametersDto)
   
        {
            Console.WriteLine(crawlingParametersDto);
            await Clients.All.SendAsync("SendingParameters", crawlingParametersDto);
          
        }
       
        public async Task GetMessages(string message)
        {
            Console.WriteLine(message);
            await Clients.All.SendAsync("ReceiveMessages", message);
        }

    }
}
