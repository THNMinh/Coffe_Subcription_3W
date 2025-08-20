using Core.Interfaces.Repositories;
using Core.Models;

namespace Repository.Repositories
{
    public class DailyCupTrackingRepository : GenericRepository<DailyCupTracking, int>, IDailyCupTrackingRepository
    {
        private readonly CoffeSubContext _context;
        public DailyCupTrackingRepository(CoffeSubContext context) : base(context)
        {
            _context = context;
        }

        //public async Task<bool> ExistsAsync(int subscriptionId, DateTime date)
        //{
        //    return await _context.DailyCupTrackings
        //        .AnyAsync(d => d.SubscriptionId == subscriptionId &&
        //                       d.Date.ToDateTime(TimeOnly.MinValue) == date);
        //}

        public async Task<bool> ExistsAsync(int subscriptionId, DateTime date)
        {
            return _context.DailyCupTrackings
                .AsEnumerable()
                .Any(d => d.SubscriptionId == subscriptionId && d.Date.ToDateTime(TimeOnly.MinValue) == date);
        }

        public async Task CreateAsync(DailyCupTracking tracking)
        {
            await _context.DailyCupTrackings.AddAsync(tracking);
            await _context.SaveChangesAsync();
        }


    }
}