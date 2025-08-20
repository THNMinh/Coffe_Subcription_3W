using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;

namespace Service.Services
{
    public class UserSubcriptionService : IUserSubcriptionService
    {
        private readonly IUserSubcriptionRepository _userSubcriptionRepository;
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly IDailyCupTrackingService _dailyCupTrackingService;

        public UserSubcriptionService(IUserSubcriptionRepository userSubcriptionRepository, ISubscriptionPlanRepository subscriptionPlanRepository, IDailyCupTrackingService dailyCupTrackingService)
        {
            _userSubcriptionRepository = userSubcriptionRepository;
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _dailyCupTrackingService = dailyCupTrackingService;
        }

        public async Task<UserSubscription> CreateAsync(UserSubscription subscription)
        {
            // First create the subscription
            var createdSubscription = await _userSubcriptionRepository.CreateAsync(subscription);

            // Then create the initial daily tracking record
            await CreateInitialDailyTracking(createdSubscription);

            return createdSubscription;
        }

        private async Task CreateInitialDailyTracking(UserSubscription subscription)
        {
            var plan = await _subscriptionPlanRepository.GetByIdAsync(subscription.PlanId);
            if (plan == null) throw new ArgumentException("Invalid plan ID");

            var initialTracking = new DailyCupTracking
            {
                SubscriptionId = subscription.SubscriptionId,
                Date = DateOnly.FromDateTime(DateTime.Today), // Fix for CS0029: Convert DateTime to DateOnly
                CupsTaken = 0,
            };

            await _dailyCupTrackingService.CreateAsync(initialTracking);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _userSubcriptionRepository.DeleteAsync(id);
        }

        public async Task<List<UserSubscription>> GetAllUserSubscriptionPlansAsync()
        {

            return await _userSubcriptionRepository.GetAllAsync();
        }

        public async Task<UserSubscription?> GetByIdAsync(int id)
        {
            return await _userSubcriptionRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(UserSubscription transaction)
        {
            return await _userSubcriptionRepository.UpdateAsync(transaction);
        }

        public async Task<UserSubscription?> GetByUserIdAsync(int id)
        {
            return await _userSubcriptionRepository.GetByIdAsync(id);
        }
    }
}
