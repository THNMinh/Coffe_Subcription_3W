namespace Core.Models;

public partial class SubscriptionPlan
{
    public int PlanId { get; set; }

    public string PlanName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int DurationDays { get; set; }

    public int TotalCups { get; set; }

    public int DailyCupLimit { get; set; }

    public bool IsActive { get; set; } = true;

    public virtual ICollection<PlanCoffeeOption> PlanCoffeeOptions { get; set; } = new List<PlanCoffeeOption>();

    public virtual ICollection<SubscriptionTimeWindow> SubscriptionTimeWindows { get; set; } = new List<SubscriptionTimeWindow>();

    public virtual ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
}
