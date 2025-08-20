using Core.Interfaces.Repositories;
using Core.Models;

namespace Repository.Repositories
{
    public class PlanCoffeeOptionRepository : GenericRepository<PlanCoffeeOption, int>, IPlanCoffeeOptionRepository
    {
        private readonly CoffeSubContext _context;
        public PlanCoffeeOptionRepository(CoffeSubContext context) : base(context)
        {
            _context = context;
        }
    }
}
