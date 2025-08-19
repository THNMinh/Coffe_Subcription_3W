using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.PlanCoffeeOptionDTO
{
    public class PlanCoffeeOptionResponseDto
    {
        public int OptionId { get; set; }

        public int PlanId { get; set; }

        public int CoffeeId { get; set; }

        public bool IsActive { get; set; }
    }
}
