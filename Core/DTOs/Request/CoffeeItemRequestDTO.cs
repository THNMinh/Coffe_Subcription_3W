using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.DTOs.Request
{
    public class CoffeeItemRequestDto
    {
        public int? CategoryId { get; set; }
        public string CoffeeName { get; set; } = null!;
        public string? Description { get; set; }
        public string Code { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public IFormFile Image { get; set; }
    }
}
