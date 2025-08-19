using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.CoffeeItemDTO
{
    public class CreateCoffeeItemDto
    {
 
        public int? CategoryId { get; set; }

        public string CoffeeName { get; set; } = null!;

        public string? Description { get; set; }

        public string Code { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public string? ImageUrl { get; set; }
    }
}
