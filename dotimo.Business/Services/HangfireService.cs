using dotimo.Business.IServices;
using dotimo.Core;
using dotimo.Data.Entities;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using dotimo.Data.Models;

namespace dotimo.Business.Services
{
    public class HangfireService : IHangfireService
    {
        private readonly IUnitOfWork<Watch> _unitOfWork;
        private readonly IWatchService _watchService;
        private readonly ILogger<HangfireService> _logger;
        private readonly INotificationService _notificationService;

        public HangfireService(IUnitOfWork<Watch> unitOfWork, IWatchService watchService, ILogger<HangfireService> logger, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _watchService = watchService;
            _logger = logger;
            _notificationService = notificationService;
        }

        public async Task CreateRecurringJobsAsync()
        {
            try
            {
                var watches = await _watchService.GetAllAsync();
                foreach (var watch in watches)
                {
                    RecurringJob.AddOrUpdate(
                        recurringJobId: string.Format("{0}", watch.Name),
                        methodCall: () => SendRequestAsync(watch),
                        cronExpression: Cron.MinuteInterval((int)watch.MonitoringTimePeriod));
                    _logger.LogInformation(string.Format(" HangfireService | RecurringJob |  {0} | Id: {1} \n\n\n", watch.Name, watch.Id));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(" HangfireService | RecurringJob | ExceptionMessage: {0}", ex.Message));
                throw;
            }
        }

        public async Task<bool> SendRequestAsync(Watch watch)
        {
            try
            {
                const int timeout = 10; // ms

                var client = new HttpClient
                {
                    Timeout = TimeSpan.FromSeconds(timeout)
                };

                var response = await client.GetAsync(watch.UrlString);

                CheckUp checkUp = new CheckUp
                {
                    StatusCode = (short)response.StatusCode,
                    UpdatedDate = DateTime.Now
                };

                if ((response.StatusCode >= (HttpStatusCode)300) || (response.StatusCode < (HttpStatusCode)200))
                {
                    _logger.LogInformation(string.Format(" Hangfire Service | SendRequestAsync | Ping Failed! URL: {0}  STATUS: {1}",
                        watch.UrlString, response.StatusCode.ToString()));
                    _notificationService.Send(new Notification());
                    return false;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(" Hangfire Service | SendRequestAsync | Request Failed! URL: {0}, ExceptionMessage: {1}", watch.UrlString, ex.Message));
                _notificationService.Send(new Notification());
                throw;
            }
            return true;
        }

        public bool PingUrl(Watch watch)
        {
            try
            {
                const int timeout = 10000; // ms
                var ping = new Ping();

                var result = ping.SendPingAsync(watch.UrlString, timeout).Result;
                if (result.Status != IPStatus.Success)
                {
                    _logger.LogInformation(string.Format("Ping Failed! URL: {0}  STATUS: {1}", watch.UrlString, result.Status));
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("URL: {0}, ExceptionMessage: {1}", watch.UrlString, ex.Message));
                throw;
            }
            return true;
        }
    }
}