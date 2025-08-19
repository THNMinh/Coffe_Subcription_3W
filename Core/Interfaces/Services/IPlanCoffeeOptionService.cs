using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IPlanCoffeeOptionService
    {
        Task<List<PlanCoffeeOption>> GetAllPlanCoffeeOptionAsync();
        Task<PlanCoffeeOption?> GetByIdAsync(int id);

        Task<PlanCoffeeOption> CreateAsync(PlanCoffeeOption option);

        Task<bool> UpdateAsync(PlanCoffeeOption option);

        Task<bool> DeleteAsync(int id);
    }
}
