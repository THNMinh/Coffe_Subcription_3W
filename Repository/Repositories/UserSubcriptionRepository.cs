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
        
    }
}
