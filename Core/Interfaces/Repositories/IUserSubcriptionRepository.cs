using Core.Interfaces.Repository;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IUserSubcriptionRepository : IGenericRepository<UserSubscription, int>
    {
        Task<List<UserSubscription>> GetActiveSubscriptionsAsync(DateTime forDate);
        Task<UserSubscription?> GetActiveSubscriptionAsync(int subscriptionId);
        Task<bool> IsCoffeeInPlanAsync(int planId, int coffeeId);
        Task<bool> IsWithinValidTimeWindowAsync(int planId, TimeSpan time);
        Task DecrementRemainingCupsAsync(int subscriptionId);

    }
}
