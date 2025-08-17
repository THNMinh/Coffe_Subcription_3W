using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;
using Core.Models;

namespace Core.Interfaces.Services
{
    public interface ISubscriptionTimeWindowService
    {
        Task<List<SubscriptionTimeWindowDTO>> GetAllSubscriptionTimeWindowsAsync();
        Task<SubscriptionTimeWindowDTO?> GetByIdAsync(int id);

        Task<SubscriptionTimeWindowDTO> CreateAsync(SubscriptionTimeWindow timeWindow);

        Task<bool> UpdateAsync(SubscriptionTimeWindow timeWindow);

        Task<bool> DeleteAsync(int id);
    }
}
