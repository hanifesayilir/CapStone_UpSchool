using Application.Features.Notifications.Queries.GetByUserId;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface INotificationApplicationHubService
    {

        Task SendApplication(NotificationGetByUserIdDto notification, CancellationToken cancellationToken);
    }
}
