using Core.Interfaces.Repository;
using Core.Models;

namespace Core.Interfaces.Repositories
{
    public interface ISubscriptionPlanRepository : IGenericRepository<SubscriptionPlan, int>
    {
        Task<List<SubscriptionPlan>> GetAllWithDetailsAsync();
        Task<List<SubscriptionPlan>> GetByIdWithDetailsAsync(int id);


    }
}
