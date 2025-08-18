using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class CoffeeItemRepository : GenericRepository< CoffeeItem, int>,ICoffeeItemRepository
    {
        private readonly CoffeSubContext  _context;
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

    }
}
