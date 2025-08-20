using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.UserSubscriptionDTO
{
    public class UserSubscriptionProfileResponseDTO
    {
        public int SubscriptionId { get; set; }

        public int PlanId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int RemainingCups { get; set; }

        public bool IsActive { get; set; }
    }
}
