using Core.Interfaces.Repositories;
using Core.Models;

namespace Repository.Repositories
{
    public class SubscriptionTimeWindowRepository : GenericRepository<SubscriptionTimeWindow, int>, ISubscriptionTimeWindowRepository
    {
        private readonly CoffeSubContext _context;
        public SubscriptionTimeWindowRepository(CoffeSubContext context) : base(context)
        {
            _context = context;
        }
    }
}
