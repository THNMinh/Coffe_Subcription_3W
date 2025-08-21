using Core.DTOs;
using Core.Models;

namespace Core.Interfaces.Services
{
    public interface IDailyCupTrackingService
    {
        Task<List<DailyCupTrackingDTO>> GetAllDailyCupTrackingsAsync();
        Task<DailyCupTrackingDTO?> GetByIdAsync(int id);
        Task<DailyCupTracking?> GetByIdAsyncForDelete(int id);

        Task<DailyCupTracking> CreateAsync(DailyCupTracking tracking);

        Task<bool> UpdateAsync(DailyCupTracking tracking);

        Task<bool> DeleteAsync(int id);



        Task<DailyCupTracking> GetOrCreateDailyTrackingAsync(int subscriptionId, DateOnly date);
        Task IncrementUsageAsync(int trackingId);
    }
}
