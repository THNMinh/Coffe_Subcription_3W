using Core.DTOs.CoffeeRedemtionDTO;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
