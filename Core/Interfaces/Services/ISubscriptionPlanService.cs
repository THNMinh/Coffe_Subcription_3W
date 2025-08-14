using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface ISubscriptionPlanService
    {
        Task<List<SubscriptionPlan>> GetAllSubscriptionPlansAsync();
        Task<SubscriptionPlan?> GetByIdAsync(int id);

        Task<SubscriptionPlan> CreateAsync(SubscriptionPlan chapter);

        Task<bool> UpdateAsync(SubscriptionPlan chapter);

        Task<bool> DeleteAsync(int id);
    }
}
