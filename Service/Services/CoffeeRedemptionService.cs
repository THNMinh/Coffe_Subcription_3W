using Core.DTOs.CoffeeRedemtionDTO;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.Extensions.Logging;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class CoffeeRedemptionService : ICoffeeRedemptionService
    {
        private readonly ICoffeeRedemptionRepository _coffeeRedemptionRepository;
        private readonly ICoffeeItemRepository _coffeeItemRepository;
        private readonly IUserSubcriptionRepository _subscriptionRepository;
        private readonly IDailyCupTrackingService _dailyCupTrackingService;
        private readonly ILogger<CoffeeRedemptionService> _logger;

        public CoffeeRedemptionService(
            ICoffeeRedemptionRepository coffeeRedemptionRepository,
            ICoffeeItemRepository coffeeItemRepository,
            IUserSubcriptionRepository subscriptionRepository,
            IDailyCupTrackingService dailyCupTrackingService,
            ILogger<CoffeeRedemptionService> logger)
        {
            _coffeeRedemptionRepository = coffeeRedemptionRepository;
            _coffeeItemRepository = coffeeItemRepository;
            _subscriptionRepository = subscriptionRepository;
            _dailyCupTrackingService = dailyCupTrackingService;
            _logger = logger;
        }



        public async Task<CoffeeRedemptionResult> ProcessRedemptionAsync(int subscriptionId, string coffeeCode)
        {
            //var redemptionTime = DateTime.Now;
            var redemptionTime = DateTime.UtcNow;
            var result = new CoffeeRedemptionResult();

            try
            {
                // 1. Get subscription and validate basic info
                var subscription = await _subscriptionRepository.GetActiveSubscriptionAsync(subscriptionId);
                if (subscription == null)
                {
                    result.FailureReason = "Invalid or inactive subscription";
                    return result;
                }

                // 2. Get coffee item by code
                var coffeeItem = await _coffeeItemRepository.GetByCodeAsync(coffeeCode);
                if (coffeeItem == null)
                {
                    result.FailureReason = "Invalid coffee code";
                    return result;
                }

                // 3. Validate coffee is in plan
                if (!await _subscriptionRepository.IsCoffeeInPlanAsync(subscription.PlanId, coffeeItem.CoffeeId))
                {
                    result.FailureReason = "This coffee is not available in your subscription plan";
                    return result;
                }

                // 4. Validate time window
                var isValidTime = await _subscriptionRepository.IsWithinValidTimeWindowAsync(
                    subscription.PlanId,
                    redemptionTime.TimeOfDay);

                if (!isValidTime)
                {
                    result.FailureReason = "Redemption outside allowed time window";
                    return result;
                }

                // 5. Check daily cup limit
                var dailyTracking = await _dailyCupTrackingService.GetOrCreateDailyTrackingAsync(
                    subscriptionId,
                    DateOnly.FromDateTime(redemptionTime));

                if (dailyTracking.CupsTaken >= subscription.Plan.DailyCupLimit)
                {
                    result.FailureReason = "Daily cup limit reached";
                    return result;
                }

                // 6. Check remaining cups
                if (subscription.RemainingCups <= 0)
                {
                    result.FailureReason = "No remaining cups in subscription";
                    return result;
                }

                // 7. Create redemption record
                var redemption = new CoffeeRedemption
                {
                    SubscriptionId = subscriptionId,
                    CoffeeId = coffeeItem.CoffeeId,
                    RedemptionTime = redemptionTime,
                    CodeUsed = coffeeCode,
                    IsSuccessful = true
                };

                await _coffeeRedemptionRepository.CreateAsync(redemption);

                // 8. Update tracking and subscription
                await _dailyCupTrackingService.IncrementUsageAsync(dailyTracking.TrackingId);
                await _subscriptionRepository.DecrementRemainingCupsAsync(subscriptionId);

                result.IsSuccess = true;
                //result.Redemption = redemption;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing redemption");
                result.FailureReason = "System error processing redemption";
                return result;
            }
        }

       



        public async Task<CoffeeRedemption> CreateAsync(CoffeeRedemption coffeeRedemption)
        {
            return await _coffeeRedemptionRepository.CreateAsync(coffeeRedemption);
        }

        public async Task<bool> DeleteAsync(int id)
        {

            return await _coffeeRedemptionRepository.DeleteAsync(id);
        }

        public async Task<List<CoffeeRedemption>> GetAllCoffeeRedemptionPlansAsync()
        {
            return await _coffeeRedemptionRepository.GetAllAsync();
        }

        public async Task<CoffeeRedemption?> GetByIdAsync(int id)
        {
            return await _coffeeRedemptionRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(CoffeeRedemption coffee)
        {
            return await _coffeeRedemptionRepository.UpdateAsync(coffee);
        }
    }
}
