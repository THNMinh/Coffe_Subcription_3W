using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.DTOs.CoffeeItemDTO
{
    public class CoffeeItemDTO
    {
        public int CoffeeId { get; set; }

        public int? CategoryId { get; set; }

        public string CoffeeName { get; set; } = null!;

        public string? Description { get; set; }

        public string Code { get; set; } = null!;

        public bool IsActive { get; set; }

        public string? ImageUrl { get; set; }

        public virtual Category? Category { get; set; }
    }
}
