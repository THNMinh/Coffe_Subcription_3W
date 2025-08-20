using Core.Interfaces.Repository;
using Core.Models;

namespace Core.Interfaces.Repositories
{
    public interface IDailyCupTrackingRepository : IGenericRepository<DailyCupTracking, int>
    {
        Task<bool> ExistsAsync(int subscriptionId, DateTime date);
        Task CreateAsync(DailyCupTracking tracking);
    }
}
