using Application.Common.Interfaces;
using Application.Features.OrderEvents.Commands.Add;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Services
{
    public class OrderEventHubManager : IOrderEventHubService
    {
        private readonly IHubContext<OrderEventHub> _hubContext;

        public OrderEventHubManager(IHubContext<OrderEventHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [Authorize]
        public  async Task AddedAsync(OrderEventHubDto orderEvent, CancellationToken cancellationToken)
        {
             await _hubContext.Clients.All.SendAsync("Added", orderEvent, cancellationToken);
            
        }
    }
}
