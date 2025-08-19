using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.CoffeeItemDTO
{
    public class CoffeeSubscriptionInfoDto
    {
        public int SubscriptionId { get; set; }
        public string CoffeeCode { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
