using Core.DTOs.UserSubscriptionDTO;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace WebAPI.Controllers
{
    public class CoffeeRedemptionController : ControllerBase
    {
        private readonly ICoffeeRedemptionService _service;

        public CoffeeRedemptionController(ICoffeeRedemptionService service)
        {
            _service = service;
        }
        [HttpPost("/api/EnterCafeCode")]
        public async Task<IActionResult> Create(int subscriptionId, string coffeeCode)
        {


           var createdSub = await _service.ProcessRedemptionAsync(subscriptionId, coffeeCode);





            return Ok(createdSub);

        }

    }
}
