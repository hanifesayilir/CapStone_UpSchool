using Application.Common.Interfaces;
using Application.Features.Notifications.Queries.GetByUserId;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Services
{
    public class NotificationApplicationHubManager : INotificationApplicationHubService
    {

        private readonly IHubContext<NotificationApplicationHub>  _hubContext;

        public NotificationApplicationHubManager(IHubContext<NotificationApplicationHub> hubContext)
        {
            _hubContext= hubContext;
        }

        [Authorize]
        public async Task SendApplication(NotificationGetByUserIdDto message, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync("SendApplicationNotifications", message, cancellationToken);
        }
    }
}
