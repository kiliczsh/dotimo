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
                var credentials = new NetworkCredential(
                    userName: Environment.GetEnvironmentVariable("MAIL_USERNAME"),
                    password: Environment.GetEnvironmentVariable("MAIL_PASSWORD"));

                SmtpClient mailClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = credentials
                };

                var mail = CreateMail(notification);
                mailClient.Send(mail);
                _logger.LogInformation(" NotificationService | Email sent!");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(" NotificationService | Email failed!");
                throw;
            }
        }

        private MailMessage CreateMail(Notification notification)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(Environment.GetEnvironmentVariable("MAIL_USERNAME"), "Dotimo Alert"),
                Subject = "Dotimo | Your website is DOWN!",
                IsBodyHtml = true,
                Body = "This mail sent by Dotimo to alert you!"
            };
            mail.To.Add("<email>");
            return mail;

        }
    }
}