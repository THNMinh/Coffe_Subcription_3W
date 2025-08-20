namespace Core.DTOs.Request
{
    public class SubscriptionTimeWindowRequestDTO
    {
        public int? PlanId { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public string? Description { get; set; }
    }
}
