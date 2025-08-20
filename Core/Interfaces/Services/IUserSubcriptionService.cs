using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
