using Application.Common.Interfaces;
using Application.Common.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Helpers;
using Domain.Identity;

namespace Infrastructure.Services
{
    public class EmailManager : IEmailService
    {

        private readonly string _wwwrootPath;
        public EmailManager(string wwwrootPath)
        {
            _wwwrootPath = wwwrootPath;
        }
       public void SendEmailConfirmation(SendEmailConfirmationDto sendEmailConfirmationDto)
        {
           var htmlContent = File.ReadAllText($"{_wwwrootPath}\\email_templates\\email_confirmation.html");

            htmlContent = htmlContent.Replace("{{subject}}", MessagesHelper.Email.Confirmation.Subject);

            htmlContent = htmlContent.Replace("{{name}}", MessagesHelper.Email.Confirmation.Name(sendEmailConfirmationDto.Name));

            htmlContent = htmlContent.Replace("{{activationMessage}}", MessagesHelper.Email.Confirmation.ActivationMessage);

            htmlContent = htmlContent.Replace("{{buttonText}}", MessagesHelper.Email.Confirmation.ButtonText);

            htmlContent = htmlContent.Replace("{{buttonLink}}", MessagesHelper.Email.Confirmation.Buttonlink(sendEmailConfirmationDto.Email, sendEmailConfirmationDto.Token));

            Send(new SendEmailDto(sendEmailConfirmationDto.Email, htmlContent, MessagesHelper.Email.Confirmation.Subject));

        }

        public void SendEmailNotification(SendEmailNotificationDto sendEmailNotificationDto)
        {
            var htmlContent = File.ReadAllText($"{_wwwrootPath}\\email_templates\\email_notification_crawl.html");



            htmlContent = htmlContent.Replace("{{subject}}", MessagesHelper.Email.NotificationCrawler.Subject);

            htmlContent = htmlContent.Replace("{{name}}", MessagesHelper.Email.NotificationCrawler.Name(sendEmailNotificationDto.Name));

            htmlContent = htmlContent.Replace("{{Message}}", MessagesHelper.Email.NotificationCrawler.Message);


            htmlContent = htmlContent.Replace("{{OrderNumber}}", MessagesHelper.Email.NotificationCrawler.OrderNumber(sendEmailNotificationDto.OrderNumber));

            Send(new SendEmailDto(sendEmailNotificationDto.Email, htmlContent, MessagesHelper.Email.NotificationCrawler.Subject));
        }


        private void Send(SendEmailDto sendEmailDto)
        {
            MailMessage message = new MailMessage();

            sendEmailDto.EmailAddresses.ForEach(emailAddress => message.To.Add(emailAddress));

            message.From = new MailAddress("finalcrawlmaster@outlook.com");

            message.Subject = sendEmailDto.Subject;

            message.IsBodyHtml = true;

            message.Body = sendEmailDto.Content;

            SmtpClient client = new SmtpClient();

            client.Port = 587;

            client.Host = "smtp-mail.outlook.com";

            client.EnableSsl = true;

            client.UseDefaultCredentials = false;

            client.Credentials = new NetworkCredential("finalcrawlmaster@outlook.com", "");

            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            client.Send(message);


        }
    }
}
