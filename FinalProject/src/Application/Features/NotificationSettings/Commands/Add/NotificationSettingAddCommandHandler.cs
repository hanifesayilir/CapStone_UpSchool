using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;

namespace Application.Features.NotificationSettings.Commands.Add
{
    public class NotificationSettingAddCommandHandler : IRequestHandler<NotificationSettingAddCommand, Response<Guid>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationDbContext _applicationDbContext;
        private string _notificationSettingMessage = string.Empty;

        public NotificationSettingAddCommandHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Response<Guid>> Handle(NotificationSettingAddCommand request, CancellationToken cancellationToken)
        {

            var notificationSetting = new NotificationSetting()
            {  
                UserId = _currentUserService.UserId,  
            };

            var existedSetting = _applicationDbContext.NotificationSettings.Where(x => x.UserId == _currentUserService.UserId).FirstOrDefault();


            if (existedSetting == null) {
                _notificationSettingMessage = "Notificationsetting is added.";

                notificationSetting = new NotificationSetting()
                {
                    Id = Guid.NewGuid(),
                    IsApplicationEnabled = request.IsApplicationEnabled,
                    IsEmailEnabled = request.IsEmailEnabled,
                    UserId = _currentUserService.UserId,
                    CreatedOn = DateTimeOffset.Now,
                    CreatedByUserId = _currentUserService.UserId,
                    IsDeleted = false,
                };
                await _applicationDbContext.NotificationSettings.AddAsync(notificationSetting, cancellationToken);
                
            } 
            else
            {
                _notificationSettingMessage = "Current notificationsetting is updated.";
                existedSetting.IsApplicationEnabled = request.IsApplicationEnabled;
                existedSetting.IsEmailEnabled = request.IsEmailEnabled;
                existedSetting.UserId =_currentUserService.UserId;

                _applicationDbContext.NotificationSettings.Update(existedSetting);

               
            }
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Response<Guid>(_notificationSettingMessage, notificationSetting.Id);

        }
    }
}
