using System;
using System.Collections.Generic;

namespace Core.Models;

public partial class SubscriptionTimeWindow
{
    public int WindowId { get; set; }

    public int PlanId { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string? Description { get; set; }

    public virtual SubscriptionPlan Plan { get; set; } = null!;
}
