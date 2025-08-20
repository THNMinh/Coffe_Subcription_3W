using Core.DTOs.CoffeeItemDTO;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class CoffeeItemService : ICoffeeItemService
    {
        private readonly ICoffeeItemRepository _coffeeItemRepository;

        public CoffeeItemService(ICoffeeItemRepository coffeeItemRepository)
        {
            _coffeeItemRepository = coffeeItemRepository;
        }

        public async Task<CoffeeItem> CreateAsync(CoffeeItem chapter)
        {
            return await _coffeeItemRepository.CreateAsync(chapter);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _coffeeItemRepository.DeleteAsync(id);
        }

        public async Task<List<CoffeeItem>> GetAllCoffeeItemAsync()
        {
            return await _coffeeItemRepository.GetAllAsync();
        }

        public async Task<CoffeeItem?> GetByIdAsync(int id)
        {
            return await _coffeeItemRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(CoffeeItem chapter)
        {
            return await _coffeeItemRepository.UpdateAsync(chapter);
        }

        public Task<CoffeeSubscriptionInfoDto?> GetCoffeeSubscriptionInfoAsync(int userId, int coffeeId)
        {
            return _coffeeItemRepository.GetCoffeeSubscriptionInfoAsync(userId, coffeeId);
        }

    }
}
