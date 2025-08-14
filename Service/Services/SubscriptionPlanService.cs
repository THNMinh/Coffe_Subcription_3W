using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<SubscriptionPlan?> GetByIdAsync(int id)
        {
            return await _subscriptionPlanRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(SubscriptionPlan plan)
        {
            await _subscriptionPlanRepository.UpdateAsync(plan);
            return true;
        }
    }
}
