using System;
using System.Collections.Generic;

namespace Core.Models;

public partial class PlanCoffeeOption
{
    public int OptionId { get; set; }

    public int PlanId { get; set; }

    public int CoffeeId { get; set; }

    public bool IsActive { get; set; } = true;

    public virtual CoffeeItem Coffee { get; set; } = null!;

    public virtual SubscriptionPlan Plan { get; set; } = null!;
}
