namespace Core.DTOs.CoffeeItemDTO
{
    public class CoffeeSubscriptionInfoDto
    {
        public int SubscriptionId { get; set; }
        public string CoffeeCode { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
