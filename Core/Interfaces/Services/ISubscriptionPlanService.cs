using Core.Models;

namespace Core.Interfaces.Services
{
    public interface ISubscriptionPlanService
    {
        Task<List<SubscriptionPlan>> GetAllSubscriptionPlansAsync();
        Task<SubscriptionPlan?> GetByIdAsync(int id);

        Task<SubscriptionPlan> CreateAsync(SubscriptionPlan chapter);

        Task<bool> UpdateAsync(SubscriptionPlan chapter);

        Task<bool> DeleteAsync(int id);

        Task<List<SubscriptionPlan>> GetAllSubscriptionPlanslWithDetailsAsync();
        Task<SubscriptionPlan?> GetByIdWithDetailsAsync(int id);
    }
}
