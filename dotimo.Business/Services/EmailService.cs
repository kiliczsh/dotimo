using dotimo.Business.IServices;
using dotimo.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;

namespace dotimo.Business.Services
{
    public class EmailService : INotificationService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public void Send(Notification notification)
        {
            try
            {
                var userName = Environment.GetEnvironmentVariable("MAIL_USERNAME");
                var password = Environment.GetEnvironmentVariable("MAIL_PASSWORD");

                MailMessage mail = CreateMail(notification);
                SmtpClient mailClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(userName, password),
                    EnableSsl = true,
                    Timeout = 10000
                };

                mailClient.Send(mail);

                _logger.LogInformation(string.Format(" NotificationService | Email sent to {0}!", notification.ToEmail));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(" NotificationService | Email failed! | Details: {0} ", ex.Message);
                throw;
            }
        }

        private MailMessage CreateMail(Notification notification)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(Environment.GetEnvironmentVariable("MAIL_USERNAME"), "Dotimo Alert"),
                Subject = notification.Subject,
                IsBodyHtml = true,
                Body = notification.Body
            };
            mail.To.Add(notification.ToEmail);
            return mail;
        }
    }
}