using Core.Interfaces.Repositories;
using Core.Interfaces.Repository;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
