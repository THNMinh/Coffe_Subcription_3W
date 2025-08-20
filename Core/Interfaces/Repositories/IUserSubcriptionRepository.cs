using Core.Interfaces.Repository;
using Core.Models;

namespace Core.Interfaces.Repositories
{
    public interface IUserSubcriptionRepository : IGenericRepository<UserSubscription, int>
    {
        Task<List<UserSubscription>> GetActiveSubscriptionsAsync(DateTime forDate);
        Task<UserSubscription?> GetActiveSubscriptionAsync(int subscriptionId);
        Task<bool> IsCoffeeInPlanAsync(int planId, int coffeeId);
        Task<bool> IsWithinValidTimeWindowAsync(int planId, TimeSpan time);
        Task DecrementRemainingCupsAsync(int subscriptionId);
        Task<UserSubscription?> GetByUserIdAsync(int id);
    }
}
