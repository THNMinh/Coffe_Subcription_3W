using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;

namespace Service.Services
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

        public SubscriptionPlanService(ISubscriptionPlanRepository subscriptionPlanRepository)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
        }

        public async Task<SubscriptionPlan> CreateAsync(SubscriptionPlan plan)
        {
            await _subscriptionPlanRepository.CreateAsync(plan);
            return plan;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _subscriptionPlanRepository.DeleteAsync(id);
            return true;
        }

        public async Task<List<SubscriptionPlan>> GetAllSubscriptionPlansAsync()
        {
            var plans = await _subscriptionPlanRepository.GetAllAsync();
            return plans;
        }

        public async Task<List<SubscriptionPlan>> GetAllSubscriptionPlanslWithDetailsAsync()
        {
            var plans = await _subscriptionPlanRepository.GetAllWithDetailsAsync();
            return plans;
        }



        public async Task<SubscriptionPlan?> GetByIdAsync(int id)
        {
            return await _subscriptionPlanRepository.GetByIdAsync(id);
        }

        public async Task<SubscriptionPlan?> GetByIdWithDetailsAsync(int id)
        {
            var plans = await _subscriptionPlanRepository.GetByIdWithDetailsAsync(id);
            return plans.FirstOrDefault(); // Fix: Return the first plan or null if the list is empty
        }

        public async Task<bool> UpdateAsync(SubscriptionPlan plan)
        {
            await _subscriptionPlanRepository.UpdateAsync(plan);
            return true;
        }
    }
}
