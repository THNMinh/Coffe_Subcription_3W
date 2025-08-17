using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.DTOs.Request
{
    public class DailyCupTrackingRequestDTO
    {
        public int? SubscriptionId { get; set; }

        public DateOnly? Date { get; set; }

        public int? CupsTaken { get; set; }

    }
}
