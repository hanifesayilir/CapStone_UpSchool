using Domain.Enums;

namespace Application.Features.Notifications.Queries.GetByUserId
{
    public class NotificationGetByUserIdDto
    {

        public Guid Id { get; set; }

     
        public String Content { get; set; }

        public bool IsChecked { get; set; }

        public NotificationType NotificationType { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public NotificationGetByUserIdDto(Guid id, string content, bool isChecked, NotificationType notificationType, DateTimeOffset createdOn)
        {
            Id = id;
            Content = content;
            IsChecked = isChecked;
            NotificationType = notificationType;
            CreatedOn= createdOn;
        }

        public NotificationGetByUserIdDto()
        {
        }
    }
}
