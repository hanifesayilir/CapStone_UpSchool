using Application.Common.Models.Email;

namespace Application.Common.Interfaces
{
    public interface IEmailService
    {
        void SendEmailNotification(SendEmailNotificationDto sendEmailNotificationDto);

        void SendEmailConfirmation(SendEmailConfirmationDto sendEmailConfirmationDto);
    }
} 
