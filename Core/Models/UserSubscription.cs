namespace Core.Models;

public partial class UserSubscription
{
    public int SubscriptionId { get; set; }

    public int UserId { get; set; }

    public int PlanId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int RemainingCups { get; set; }

    public bool IsActive { get; set; }

    public string? PaymentReference { get; set; }

    public DateTime? PaymentDate { get; set; }

    public decimal? PaymentAmount { get; set; }

    public virtual ICollection<CoffeeRedemption> CoffeeRedemptions { get; set; } = new List<CoffeeRedemption>();

    public virtual ICollection<DailyCupTracking> DailyCupTrackings { get; set; } = new List<DailyCupTracking>();

    public virtual SubscriptionPlan Plan { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
