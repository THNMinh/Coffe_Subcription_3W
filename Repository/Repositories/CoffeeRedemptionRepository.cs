using Core.Interfaces.Repositories;
using Core.Models;

namespace Repository.Repositories
{
    public class CoffeeRedemptionRepository : GenericRepository<CoffeeRedemption, int>, ICoffeeRedemptionRepository
    {
        private readonly CoffeSubContext _context;
        public CoffeeRedemptionRepository(CoffeSubContext context) : base(context)
        {
            _context = context;
        }
        // Additional methods specific to CoffeeRedemption can be added here


    }
}
