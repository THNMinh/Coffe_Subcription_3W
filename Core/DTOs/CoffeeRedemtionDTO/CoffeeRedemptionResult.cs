using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.CoffeeRedemtionDTO
{
    public class CoffeeRedemptionResult
    {
        public bool IsSuccess { get; set; }
        public string? FailureReason { get; set; }
        //public CoffeeRedemption? Redemption { get; set; }
    }
}
