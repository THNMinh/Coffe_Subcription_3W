using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class DailyTrackingBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<DailyTrackingBackgroundService> _logger;
        private readonly TimeSpan _runAtTime = new TimeSpan(0, 5, 0); // 12:05 AM
        private readonly bool _isTesting;

        // Add optional testing parameter (defaults to false)
        public DailyTrackingBackgroundService(
            IServiceProvider services,
            ILogger<DailyTrackingBackgroundService> logger,
            bool isTesting = false)
        {
            _services = services;
            _logger = logger;
            _isTesting = isTesting;

            if (_isTesting)
            {
                _logger.LogWarning("⚠️ DAILY TRACKING SERVICE IN TEST MODE ⚠️");
            }
        }

        //private readonly IServiceProvider _services;
        //private readonly ILogger<DailyTrackingBackgroundService> _logger;
        //private readonly TimeSpan _runAtTime = new TimeSpan(0, 5, 0); // 12:05 AM

        //public DailyTrackingBackgroundService(
        //    IServiceProvider services,
        //    ILogger<DailyTrackingBackgroundService> logger)
        //{
        //    _services = services;
        //    _logger = logger;
        //}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Daily Tracking Background Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;
                    var nextRun = now.Date.AddDays(1).Add(_runAtTime);
                    var delay = nextRun - now;

                    _logger.LogInformation("Next run at {NextRun}", nextRun);

                    await Task.Delay(delay, stoppingToken);
                    await CreateNextDayRecordsAsync();
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Service stopping");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing daily tracking");
                    // Wait 5 minutes before retrying on error
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }

        private async Task CreateNextDayRecordsAsync()
        {
            using var scope = _services.CreateScope();
            var subscriptionRepo = scope.ServiceProvider
                .GetRequiredService<IUserSubcriptionRepository>();
            var trackingRepo = scope.ServiceProvider
                .GetRequiredService<IDailyCupTrackingRepository>();
            var logger = scope.ServiceProvider
                .GetRequiredService<ILogger<DailyTrackingBackgroundService>>();

            var tomorrow = DateTime.Today.AddDays(1);
            var tomorrowDateOnly = DateOnly.FromDateTime(tomorrow); // Convert DateTime to DateOnly
            logger.LogInformation("Creating daily tracking records for {Date}", tomorrowDateOnly);

            var activeSubscriptions = await subscriptionRepo
                .GetActiveSubscriptionsAsync(tomorrow); // Keep DateTime for repository method

            foreach (var sub in activeSubscriptions)
            {
                try
                {
                    if (!await trackingRepo.ExistsAsync(sub.SubscriptionId, tomorrow)) // Keep DateTime for repository method
                    {
                        var newRecord = new DailyCupTracking
                        {
                            SubscriptionId = sub.SubscriptionId,
                            Date = tomorrowDateOnly, // Use DateOnly for the DailyCupTracking object
                            CupsTaken = 0,
                        };

                        await trackingRepo.CreateAsync(newRecord);
                        logger.LogDebug("Created record for subscription {SubscriptionId}", sub.SubscriptionId);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex,
                        "Failed to create tracking record for subscription {SubscriptionId}",
                        sub.SubscriptionId);
                }
            }

            logger.LogInformation("Created {Count} daily tracking records",
                activeSubscriptions.Count);
        }

        //Cai nay la de test
        public async Task ForceCreateRecordsForTestAsync(DateTime testDate, int advanceDays = 1)
        {
            using var scope = _services.CreateScope();
            var subscriptionRepo = scope.ServiceProvider
                .GetRequiredService<IUserSubcriptionRepository>();
            var trackingRepo = scope.ServiceProvider
                .GetRequiredService<IDailyCupTrackingRepository>();

            var targetDate = testDate.AddDays(advanceDays);
            var targetDateOnly = DateOnly.FromDateTime(targetDate); // Convert DateTime to DateOnly
            _logger.LogInformation("[TEST MODE] Creating records for {Date}", targetDateOnly);

            var activeSubscriptions = await subscriptionRepo
                .GetActiveSubscriptionsAsync(targetDate); // Keep DateTime for repository method

            foreach (var sub in activeSubscriptions)
            {
                if (!await trackingRepo.ExistsAsync(sub.SubscriptionId, targetDate)) // Keep DateTime for repository method
                {
                    await trackingRepo.CreateAsync(new DailyCupTracking
                    {
                        SubscriptionId = sub.SubscriptionId,
                        Date = targetDateOnly, // Use DateOnly for the DailyCupTracking object
                        CupsTaken = 20,
                        //DailyLimit = sub.Plan.DailyCupLimit
                    });
                }
            }
        }
    }



}
