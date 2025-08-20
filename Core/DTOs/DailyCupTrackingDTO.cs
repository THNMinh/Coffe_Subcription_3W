namespace Core.DTOs
{
    public class DailyCupTrackingDTO
    {
        public int TrackingId { get; set; }

        public int SubscriptionId { get; set; }

        public DateOnly Date { get; set; }

        public int CupsTaken { get; set; }

    }
}
