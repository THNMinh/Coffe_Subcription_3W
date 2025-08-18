using Core.DTOs;
using Core.DTOs.Request;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class DailyCupTrackingService : BackgroundService, IDailyCupTrackingService
    {
        private readonly IDailyCupTrackingRepository _repo;
        private readonly CoffeSubContext _context;
        private readonly IUserSubcriptionRepository _userSubscriptionRepository;
        private readonly ILogger<DailyCupTrackingService> _logger;

        public DailyCupTrackingService(IDailyCupTrackingRepository repo, CoffeSubContext context, IUserSubcriptionRepository userSubscriptionRepository, ILogger<DailyCupTrackingService> logger)
        {
            _repo = repo;
            _context = context;
            _userSubscriptionRepository = userSubscriptionRepository;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var tomorrow = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

                // Get active subscriptions that will be valid tomorrow
                var activeSubs = _context.UserSubscriptions
                    .Where(us => us.IsActive &&
                                us.EndDate >= tomorrow.ToDateTime(TimeOnly.MinValue) &&
                                us.RemainingCups > 0)
                    .ToList();

                foreach (var sub in activeSubs)
                {
                    // Check if record already exists
                    var exists = _context.DailyCupTrackings
                        .Any(d => d.SubscriptionId == sub.SubscriptionId &&
                                 d.Date == tomorrow);

                    if (!exists)
                    {
                        _context.DailyCupTrackings.Add(new DailyCupTracking
                        {
                            SubscriptionId = sub.SubscriptionId,
                            Date = tomorrow,
                            CupsTaken = 0
                        });
                    }
                }

                await _context.SaveChangesAsync();

                // Wait until next day
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        public async Task<DailyCupTracking> GetOrCreateDailyTrackingAsync(int subscriptionId, DateOnly date)
        {
            var tracking = await _context.DailyCupTrackings
                .FirstOrDefaultAsync(d =>
                    d.SubscriptionId == subscriptionId &&
                    d.Date == date);

            if (tracking == null)
            {
                //var subscription = await _context.UserSubscriptions
                //    .Include(us => us.Plan)
                //    .FirstOrDefaultAsync(us => us.SubscriptionId == subscriptionId);

                //if (subscription == null)
                //    throw new ArgumentException("Invalid subscription ID");

                tracking = new DailyCupTracking
                {
                    SubscriptionId = subscriptionId,
                    Date = date,
                    CupsTaken = 0,
                    //DailyLimit = subscription.Plan.DailyCupLimit
                };

                _context.DailyCupTrackings.Add(tracking);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Created new daily tracking for subscription {SubscriptionId}", subscriptionId);
            }

            return tracking;
        }

        public async Task IncrementUsageAsync(int trackingId)
        {
            var tracking = await _context.DailyCupTrackings
                .FirstOrDefaultAsync(d => d.TrackingId == trackingId);

            if (tracking != null)
            {
                tracking.CupsTaken++;
                await _context.SaveChangesAsync();
            }
        }



        public async Task<List<DailyCupTrackingDTO>> GetAllDailyCupTrackingsAsync()
        {
            var trackings = await _repo.GetAllAsync();
            var dtos = trackings.Adapt<List<DailyCupTrackingDTO>>();
            return dtos;
        }

        public async Task<DailyCupTrackingDTO?> GetByIdAsync(int id)
        {
            var tracking = await _repo.GetByIdAsync(id);
            var dtos = tracking.Adapt<DailyCupTrackingDTO>();
            return dtos;
        }

        public async Task<DailyCupTracking> CreateAsync(DailyCupTracking tracking)
        {
            await _repo.CreateAsync(tracking);
            //var dtos = tracking.Adapt<DailyCupTrackingDTO>();
            return tracking;
        }

        public async Task<bool> UpdateAsync(DailyCupTracking tracking)
        {
            await _repo.UpdateAsync(tracking);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
            return true;
        }
    }
}
