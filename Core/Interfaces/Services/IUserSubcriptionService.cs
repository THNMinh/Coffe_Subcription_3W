using Core.Models;

namespace Core.Interfaces.Services
{
    public interface IUserSubcriptionService
    {
        Task<List<UserSubscription>> GetAllUserSubscriptionPlansAsync();
        Task<UserSubscription?> GetByIdAsync(int id);
        Task<UserSubscription> CreateAsync(UserSubscription transaction);
        Task<bool> UpdateAsync(UserSubscription transaction);
        Task<bool> DeleteAsync(int id);
        Task<UserSubscription?> GetByUserIdAsync(int id);
    }
}
