namespace Core.DTOs.CoffeeItemDTO
{
    public class CoffeeItemResponseDto
    {
        public int CoffeeId { get; set; }

        public int? CategoryId { get; set; }

        public string CoffeeName { get; set; } = null!;

        public string? Description { get; set; }

        public string Code { get; set; } = null!;

        public bool IsActive { get; set; }

        public string? ImageUrl { get; set; }
    }
}
