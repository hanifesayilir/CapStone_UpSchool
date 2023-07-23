using MediatR;

namespace Application.Features.Notifications.Queries.GetByUserId
{
    public class NotificationGetByUserIdQuery :IRequest<List<NotificationGetByUserIdDto>>
    {

    }
}
