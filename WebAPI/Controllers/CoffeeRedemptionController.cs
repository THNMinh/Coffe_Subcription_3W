using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeRedemptionController : ControllerBase
    {
        private readonly ICoffeeRedemptionService _service;

        public CoffeeRedemptionController(ICoffeeRedemptionService service)
        {
            _service = service;
        }
        [HttpPost("")]
        public async Task<IActionResult> Create(int subscriptionId, string coffeeCode)
        {
            var createdSub = await _service.ProcessRedemptionAsync(subscriptionId, coffeeCode);
            return Ok(createdSub);
        }

    }
}
