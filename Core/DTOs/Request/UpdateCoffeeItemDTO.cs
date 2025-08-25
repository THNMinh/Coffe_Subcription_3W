using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.DTOs.Request
{
    public class UpdateCoffeeItemDTO
    {
        public string? CoffeeName { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public bool? IsActive { get; set; } = true;
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
    }
}
