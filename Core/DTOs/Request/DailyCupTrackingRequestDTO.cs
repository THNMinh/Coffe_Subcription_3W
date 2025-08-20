namespace Core.DTOs.Request
{
    public class DailyCupTrackingRequestDTO
    {
        public int? SubscriptionId { get; set; }

        public DateOnly? Date { get; set; }

        public int? CupsTaken { get; set; }

    }
}
