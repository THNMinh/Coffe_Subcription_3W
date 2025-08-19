using Core.DTOs.CoffeeItemDTO;
using Core.Interfaces.Repository;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ICoffeeItemRepository : IGenericRepository<CoffeeItem, int>
    {
        public Task<int?> GetCoffeeIdByCodeAsync(string code);

        Task<CoffeeItem?> GetByCodeAsync(string code);

        Task<CoffeeSubscriptionInfoDto?> GetCoffeeSubscriptionInfoAsync(int userId, int coffeeId);

    }
}
