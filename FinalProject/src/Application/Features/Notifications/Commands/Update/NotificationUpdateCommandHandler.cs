using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc8032;

namespace Application.Features.Notifications.Commands.Update
{
    public class NotificationUpdateCommandHandler : IRequestHandler<NotificationUpdateCommand, Response<Guid>>
    {

        public readonly IApplicationDbContext _applicationDbContext;

        public readonly ICurrentUserService _currentUserService;

        public NotificationUpdateCommandHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
        }
        public async Task<Response<Guid>> Handle(NotificationUpdateCommand request, CancellationToken cancellationToken)
        {


            var notification = await _applicationDbContext.Notifications.FirstOrDefaultAsync(x => x.Id == request.Id);
               
            if (notification == null) { throw new ArgumentNullException(nameof(notification.Id)); }

            notification.IsChecked = true;
            notification.ModifiedOn = DateTimeOffset.Now;
            notification.ModifiedByUserId = _currentUserService.UserId;

            _applicationDbContext.Notifications.Update(notification);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Response<Guid>($"The {notification.Id} notification was successfully updated", notification.Id);



        }

        
    }
}
