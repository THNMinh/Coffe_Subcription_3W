using Core.DTOs.CoffeeItemDTO;
using Core.Models;

namespace Core.Interfaces.Services
{
    public interface ICoffeeItemService
    {

        Task<List<CoffeeItem>> GetAllCoffeeItemAsync();
        Task<CoffeeItem?> GetByIdAsync(int id);

        Task<CoffeeItem> CreateAsync(CoffeeItem chapter);

        Task<bool> UpdateAsync(CoffeeItem chapter);

        Task<bool> DeleteAsync(int id);

        Task<CoffeeSubscriptionInfoDto?> GetCoffeeSubscriptionInfoAsync(int userId, int coffeeId);

    }
}
