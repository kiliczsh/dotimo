using dotimo.Business.IServices;
using dotimo.Data.Models;
using Microsoft.Extensions.Logging;
using System;

namespace dotimo.Business.Services
{
    public class SmsService : INotificationService
    {
        private readonly ILogger<SmsService> _logger;

        public SmsService(ILogger<SmsService> logger)
        {
            _logger = logger;
        }

        public void Send(Notification notification)
        {
            try
            {
                notification.ToPhone = "05001112233";
                _logger.LogInformation(string.Format(" SmsService | SMS sent to {0}!", notification.ToPhone));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(" SmsService | SMS failed! | Details: {0} ", ex.Message);
                throw;
            }
        }
    }
}