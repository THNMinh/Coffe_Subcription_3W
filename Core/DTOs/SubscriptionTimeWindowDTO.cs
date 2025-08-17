using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class SubscriptionTimeWindowDTO
    {
        public int WindowId { get; set; }

        public int PlanId { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string? Description { get; set; }
    }
}
