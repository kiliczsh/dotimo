using dotimo.Business.IServices;
using dotimo.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotimo.Business.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public void Send(Notification notification)
        {
            try
            {
                _logger.LogInformation(" NotificationService | Email sent!");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(" NotificationService | Email failed!");
            }
        }
    }
}
