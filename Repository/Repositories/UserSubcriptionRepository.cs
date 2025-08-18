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
    public class UserSubcriptionRepository : GenericRepository<UserSubscription, int>, IUserSubcriptionRepository
    {
        private readonly CoffeSubContext _context;
        public UserSubcriptionRepository(CoffeSubContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<UserSubscription>> GetActiveSubscriptionsAsync(DateTime forDate)
        {
            return await _context.UserSubscriptions
                .Include(us => us.Plan)
                .Where(us => us.IsActive &&
                            us.StartDate <= forDate &&
                            us.EndDate >= forDate &&
                            us.RemainingCups > 0)
                .ToListAsync();
        }

        public async Task<UserSubscription?> GetActiveSubscriptionAsync(int subscriptionId)
        {
            return await _context.UserSubscriptions
                .Include(us => us.Plan)
                .ThenInclude(p => p.SubscriptionTimeWindows)
                .FirstOrDefaultAsync(us =>
                    us.SubscriptionId == subscriptionId &&
                    us.IsActive &&
                    us.EndDate >= DateTime.UtcNow.Date &&
                    us.RemainingCups > 0);
        }

        public async Task<bool> IsCoffeeInPlanAsync(int planId, int coffeeId)
        {
            return await _context.PlanCoffeeOptions
                .AnyAsync(pco =>
                    pco.PlanId == planId &&
                    pco.CoffeeId == coffeeId &&
                    pco.IsActive);
        }

        public async Task<bool> IsWithinValidTimeWindowAsync(int planId, TimeSpan time)
        {
            var currentTime = TimeOnly.FromTimeSpan(time);
            return await _context.SubscriptionTimeWindows
                .AnyAsync(tw =>
                    tw.PlanId == planId &&
                    currentTime >= tw.StartTime &&
                    currentTime <= tw.EndTime);
        }

        public async Task DecrementRemainingCupsAsync(int subscriptionId)
        {
            var subscription = await _context.UserSubscriptions
                .FirstOrDefaultAsync(us => us.SubscriptionId == subscriptionId);

            if (subscription != null && subscription.RemainingCups > 0)
            {
                subscription.RemainingCups--;
                await _context.SaveChangesAsync();
            }
        }

    }
}
