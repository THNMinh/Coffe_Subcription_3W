using Core.DTOs.CoffeeItemDTO;
using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repositories
{
    public class CoffeeItemRepository : GenericRepository<CoffeeItem, int>, ICoffeeItemRepository
    {
        private readonly CoffeSubContext _context;
        public CoffeeItemRepository(CoffeSubContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int?> GetCoffeeIdByCodeAsync(string code)
        {
            return await _context.CoffeeItems
                .Where(ci => ci.Code == code && ci.IsActive)
                .Select(ci => (int?)ci.CoffeeId) // Cast to nullable int
                .FirstOrDefaultAsync();
        }

        public async Task<CoffeeItem?> GetByCodeAsync(string code)
        {
            return await _context.CoffeeItems
                .FirstOrDefaultAsync(ci =>
                    ci.Code == code &&
                    ci.IsActive);
        }

        public async Task<CoffeeSubscriptionInfoDto?> GetCoffeeSubscriptionInfoAsync(int userId, int coffeeId)
        {
            var result = await (from us in _context.UserSubscriptions
                                join ci in _context.CoffeeItems on coffeeId equals ci.CoffeeId
                                where us.UserId == userId
                                      && ci.CoffeeId == coffeeId
                                      && ci.IsActive
                                      && us.IsActive
                                select new CoffeeSubscriptionInfoDto
                                {
                                    SubscriptionId = us.SubscriptionId,
                                    CoffeeCode = ci.Code,
                                    UserId = us.UserId
                                })
                                .FirstOrDefaultAsync();

            return result;
        }


    }
}
