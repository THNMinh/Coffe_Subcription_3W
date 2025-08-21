namespace Core.Models;

public partial class DailyCupTracking : Entity
{
    public int TrackingId { get; set; }

    public int SubscriptionId { get; set; }

    public DateOnly Date { get; set; }

    public int CupsTaken { get; set; }

    public virtual UserSubscription Subscription { get; set; } = null!;
}
