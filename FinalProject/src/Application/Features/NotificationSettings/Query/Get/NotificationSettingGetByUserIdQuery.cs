using Domain.Entities;
using MediatR;

namespace Application.Features.NotificationSettings.Query.Get
{
    public class NotificationSettingGetByUserIdQuery : IRequest<NotificationSetting>
    {

    }
}
