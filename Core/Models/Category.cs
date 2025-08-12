﻿using System;
using System.Collections.Generic;

namespace Core.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<CoffeeItem> CoffeeItems { get; set; } = new List<CoffeeItem>();
}
