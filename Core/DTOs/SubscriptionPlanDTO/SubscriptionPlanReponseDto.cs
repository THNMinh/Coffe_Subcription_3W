using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.SubscriptionPlanDTO
{
    public class SubscriptionPlanReponseDto
    {
        public int PlanId { get; set; }

        public string PlanName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int DurationDays { get; set; }

        public int TotalCups { get; set; }

        public int DailyCupLimit { get; set; }

        public bool IsActive { get; set; }
    }
}
