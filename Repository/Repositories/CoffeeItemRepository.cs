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

        //public async Task<CoffeeSubscriptionInfoDto?> GetCoffeeSubscriptionInfoAsync(int userId, int coffeeId)
        //{
        //    // Check if user has any active subscription
        //    var hasActiveSubscription = await _context.UserSubscriptions
        //        .AnyAsync(us => us.UserId == userId && us.IsActive && us.EndDate.ToDateTime(TimeOnly.MinValue) >= DateTime.UtcNow);

        //    if (!hasActiveSubscription)
        //    {
        //        return new CoffeeSubscriptionInfoDto
        //        {
        //            UserId = userId,
        //            HasActiveSubscription = false,
        //            ValidationMessage = "User has no active subscription"
        //        };
        //    }

        //    // Check if coffee exists and is active
        //    var coffee = await _context.CoffeeItems
        //        .FirstOrDefaultAsync(ci => ci.CoffeeId == coffeeId && ci.IsActive);

        //    if (coffee == null)
        //    {
        //        return new CoffeeSubscriptionInfoDto
        //        {
        //            UserId = userId,
        //            HasActiveSubscription = true,
        //            ValidationMessage = "Invalid coffee item"
        //        };
        //    }

        //    // Check if coffee is in any of user's active subscriptions
        //    var subscriptionWithCoffee = await (from us in _context.UserSubscriptions
        //                                        join pco in _context.PlanCoffeeOptions on us.PlanId equals pco.PlanId
        //                                        where us.UserId == userId
        //                                              && us.IsActive
        //                                              && us.EndDate.ToDateTime(TimeOnly.MinValue) >= DateTime.UtcNow
        //                                              && pco.CoffeeId == coffeeId
        //                                              && pco.IsActive
        //                                        select new CoffeeSubscriptionInfoDto
        //                                        {
        //                                            SubscriptionId = us.SubscriptionId,
        //                                            CoffeeCode = coffee.Code,
        //                                            UserId = us.UserId,
        //                                            HasActiveSubscription = true,
        //                                            IsCoffeeInPlan = true
        //                                        })
        //                                      .FirstOrDefaultAsync();

        //    if (subscriptionWithCoffee == null)
        //    {
        //        return new CoffeeSubscriptionInfoDto
        //        {
        //            UserId = userId,
        //            HasActiveSubscription = true,
        //            IsCoffeeInPlan = false,
        //            ValidationMessage = "This coffee is not available in your subscription plan"
        //        };
        //    }

        //    return subscriptionWithCoffee;
        //}

        public async Task<CoffeeSubscriptionInfoDto?> GetCoffeeSubscriptionInfoAsync(int userId, int coffeeId)
        {
            // Check if user has any active subscription
            var hasActiveSubscription = await _context.UserSubscriptions
                .AnyAsync(us => us.UserId == userId && us.IsActive && us.EndDate >= DateTime.UtcNow.Date);

            if (!hasActiveSubscription)
            {
                return new CoffeeSubscriptionInfoDto
                {
                    UserId = userId,
                    HasActiveSubscription = false,
                    ValidationMessage = "User has no active subscription"
                };
            }

            // Check if coffee exists and is active
            var coffee = await _context.CoffeeItems
                .FirstOrDefaultAsync(ci => ci.CoffeeId == coffeeId && ci.IsActive);

            if (coffee == null)
            {
                return new CoffeeSubscriptionInfoDto
                {
                    UserId = userId,
                    HasActiveSubscription = true,
                    ValidationMessage = "Invalid coffee item"
                };
            }

            // Check if coffee is in any of user's active subscriptions
            var subscriptionWithCoffee = await (from us in _context.UserSubscriptions
                                                join pco in _context.PlanCoffeeOptions on us.PlanId equals pco.PlanId
                                                where us.UserId == userId
                                                      && us.IsActive
                                                      && us.EndDate >= DateTime.UtcNow.Date
                                                      && pco.CoffeeId == coffeeId
                                                      && pco.IsActive
                                                select new CoffeeSubscriptionInfoDto
                                                {
                                                    SubscriptionId = us.SubscriptionId,
                                                    CoffeeCode = coffee.Code,
                                                    UserId = us.UserId,
                                                    HasActiveSubscription = true,
                                                    IsCoffeeInPlan = true
                                                })
                                              .FirstOrDefaultAsync();

            if (subscriptionWithCoffee == null)
            {
                return new CoffeeSubscriptionInfoDto
                {
                    UserId = userId,
                    HasActiveSubscription = true,
                    IsCoffeeInPlan = false,
                    ValidationMessage = "This coffee is not available in your subscription plan"
                };
            }

            return subscriptionWithCoffee;
        }


    }
}
