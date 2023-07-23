using Application.Common.Interfaces;
using Application.Features.Products.Queries.GetAllByOrderId;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Notifications.Queries.GetByUserId
{
    public class NotificationGetByUserIdQueryHandler : IRequestHandler<NotificationGetByUserIdQuery, List<NotificationGetByUserIdDto>>
    {

        private readonly IApplicationDbContext _applicationDbContext;

        private readonly ICurrentUserService _currentUserService;

        public NotificationGetByUserIdQueryHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<List<NotificationGetByUserIdDto>> Handle(NotificationGetByUserIdQuery request, CancellationToken cancellationToken)
        {

            var dBQuery = _applicationDbContext.Notifications.AsQueryable();

            var notifications = await dBQuery
                 .Where(x => x.UserId == _currentUserService.UserId)
                 .Where(x => x.IsChecked == false)
                .Select(x => MapToNotificationGetByUserIdDto(x))
                .ToListAsync(cancellationToken);
         
            return notifications.ToList();         
        }

        private static NotificationGetByUserIdDto MapToNotificationGetByUserIdDto(Notification notification)
        {
            return new NotificationGetByUserIdDto()
            {
                Id = notification.Id,
                Content= notification.Content,
                IsChecked   = notification.IsChecked,
               NotificationType= notification.NotificationType,
                CreatedOn = notification.CreatedOn,
            };
        }
    }
}
