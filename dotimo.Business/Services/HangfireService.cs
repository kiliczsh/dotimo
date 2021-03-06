﻿using dotimo.Business.IServices;
using dotimo.Core;
using dotimo.Data.Entities;
using dotimo.Data.Models;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace dotimo.Business.Services
{
    public class HangfireService : IHangfireService
    {
        private readonly IUnitOfWork<Watch> _unitOfWork;
        private readonly IWatchService _watchService;
        private readonly ILogger<HangfireService> _logger;
        private readonly INotificationService _notificationService;
        private readonly ICheckUpService _checkUpService;

        public HangfireService(IUnitOfWork<Watch> unitOfWork, IWatchService watchService, ILogger<HangfireService> logger, INotificationService notificationService, ICheckUpService checkUpService)
        {
            _unitOfWork = unitOfWork;
            _watchService = watchService;
            _logger = logger;
            _notificationService = notificationService;
            _checkUpService = checkUpService;
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

                var isStatusCodeSuccess = true;

                if ((response.StatusCode >= (HttpStatusCode)300) || (response.StatusCode < (HttpStatusCode)200))
                {
                    _logger.LogInformation(string.Format(" Hangfire Service | SendRequestAsync | Ping Failed! URL: {0}  STATUS: {1}",
                        watch.UrlString, response.StatusCode.ToString()));
                    isStatusCodeSuccess = false;
                    var notification = CreateNotification(watch);
                    _notificationService.Send(notification);
                }

                CheckUp checkUp = new CheckUp
                {
                    StatusCode = (short)response.StatusCode,
                    UpdatedDate = DateTime.Now,
                    Success = isStatusCodeSuccess,
                    WatchId = watch.Id
                };
                var checkUpDb = await _checkUpService.CreateAsync(checkUp);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(" Hangfire Service | SendRequestAsync | Request Failed! URL: {0}, ExceptionMessage: {1}", watch.UrlString, ex.Message));
                var notification = CreateNotification(watch);
                _notificationService.Send(notification);
                throw;
            }
            return true;
        }

        private Notification CreateNotification(Watch watch)
        {
            var notification = new Notification
            {
                ToEmail = watch.Email,
                Subject = string.Format("Dotimo Alert | {0} is DOWN! ", watch.Name),
                Body = string.Format("<html><body> {0} is down at {1}. Please check it! </body></html>", watch.UrlString, DateTime.Now)
            };
            return notification;
        }
    }
}