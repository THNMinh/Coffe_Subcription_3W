using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repositories
{
    public class SubscriptionPlanRepository : GenericRepository<SubscriptionPlan, int>, ISubscriptionPlanRepository
    {
        private readonly CoffeSubContext _context;
        public SubscriptionPlanRepository(CoffeSubContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<SubscriptionPlan>> GetAllWithDetailsAsync()
        {
            return await _context.SubscriptionPlans
                .Include(sp => sp.SubscriptionTimeWindows)
                .Include(sp => sp.PlanCoffeeOptions)
                .ToListAsync();
        }

        public async Task<List<SubscriptionPlan>> GetByIdWithDetailsAsync(int id)
        {
            return await _context.SubscriptionPlans
                .Include(sp => sp.SubscriptionTimeWindows)
                .Include(sp => sp.PlanCoffeeOptions)
                .Where(sp => sp.PlanId == id)
                .ToListAsync();
        }


    }
}
