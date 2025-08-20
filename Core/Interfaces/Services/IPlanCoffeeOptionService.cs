using Core.Models;

namespace Core.Interfaces.Services
{
    public interface IPlanCoffeeOptionService
    {
        Task<List<PlanCoffeeOption>> GetAllPlanCoffeeOptionAsync();
        Task<PlanCoffeeOption?> GetByIdAsync(int id);

        Task<PlanCoffeeOption> CreateAsync(PlanCoffeeOption option);

        Task<bool> UpdateAsync(PlanCoffeeOption option);

        Task<bool> DeleteAsync(int id);
    }
}
