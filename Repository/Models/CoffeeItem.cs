using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class CoffeeItem
{
    public int CoffeeId { get; set; }

    public int? CategoryId { get; set; }

    public string CoffeeName { get; set; } = null!;

    public string? Description { get; set; }

    public string Code { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<CoffeeRedemption> CoffeeRedemptions { get; set; } = new List<CoffeeRedemption>();

    public virtual ICollection<PlanCoffeeOption> PlanCoffeeOptions { get; set; } = new List<PlanCoffeeOption>();
}
