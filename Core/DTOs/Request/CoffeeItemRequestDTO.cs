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
