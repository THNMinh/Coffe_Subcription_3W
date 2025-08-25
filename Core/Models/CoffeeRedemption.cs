namespace Core.Models;

public partial class CoffeeRedemption : Entity
{
    public int RedemptionId { get; set; }

    public int SubscriptionId { get; set; }

    public int CoffeeId { get; set; }

    public DateTime RedemptionTime { get; set; }

    public string CodeUsed { get; set; } = null!;

    public bool IsSuccessful { get; set; }

    public string? FailureReason { get; set; }

    public virtual CoffeeItem Coffee { get; set; } = null!;

    public virtual UserSubscription Subscription { get; set; } = null!;
}
