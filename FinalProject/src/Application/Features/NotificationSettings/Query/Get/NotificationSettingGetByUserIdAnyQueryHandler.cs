using Application.Common.Interfaces;
using Application.Features.NotificationSettings.Query.Get;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;


namespace Application.Features.NotificationSettings.Query.GetIfExist
{
    public class NotificationSettingGetByUserIdAnyQueryHandler : IRequestHandler<NotificationSettingGetByUserIdQuery, NotificationSetting>
    {

        private readonly IApplicationDbContext _applicationDbContext;

        private readonly ICurrentUserService _currentUserService;

        public NotificationSettingGetByUserIdAnyQueryHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
        }
      

        public async Task<NotificationSetting> Handle(NotificationSettingGetByUserIdQuery request, CancellationToken cancellationToken)
        {
            var existingNotification = await _applicationDbContext.NotificationSettings
                .FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId);

            return existingNotification;
        }
    }
}
