using Core.DTOs.CoffeeRedemtionDTO;
using Core.Models;

namespace Core.Interfaces.Services
{
    public interface ICoffeeRedemptionService
    {
        Task<CoffeeRedemptionResult> ProcessRedemptionAsync(int subscriptionId, string coffeeCode);

        Task<List<CoffeeRedemption>> GetAllCoffeeRedemptionPlansAsync();
        Task<CoffeeRedemption?> GetByIdAsync(int id);

        Task<CoffeeRedemption> CreateAsync(CoffeeRedemption transaction);

        Task<bool> UpdateAsync(CoffeeRedemption transaction);

        Task<bool> DeleteAsync(int id);
    }
}
