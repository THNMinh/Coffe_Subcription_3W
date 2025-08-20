using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;

namespace Service.Services
{
    public class PlanCoffeeOptionService : IPlanCoffeeOptionService
    {
        private readonly IPlanCoffeeOptionRepository _planCoffeeOptionRepository;
        public PlanCoffeeOptionService(IPlanCoffeeOptionRepository planCoffeeOptionRepository)
        {
            _planCoffeeOptionRepository = planCoffeeOptionRepository;
        }

        public async Task<PlanCoffeeOption> CreateAsync(PlanCoffeeOption option)
        {
            return await _planCoffeeOptionRepository.CreateAsync(option);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _planCoffeeOptionRepository.DeleteAsync(id);
        }

        public async Task<List<PlanCoffeeOption>> GetAllPlanCoffeeOptionAsync()
        {
            return await _planCoffeeOptionRepository.GetAllAsync();
        }

        public async Task<PlanCoffeeOption?> GetByIdAsync(int id)
        {
            return await _planCoffeeOptionRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(PlanCoffeeOption option)
        {
            return await _planCoffeeOptionRepository.UpdateAsync(option);
        }
    }
}
