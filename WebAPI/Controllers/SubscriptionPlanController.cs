using AutoMapper;
using Core.DTOs.CoffeeItemDTO;
using Core.DTOs.SubscriptionPlanDTO;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPlanController : ControllerBase
    {
        private readonly ISubscriptionPlanService _service;
        private readonly IMapper _mapper;

        public SubscriptionPlanController(ISubscriptionPlanService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetAllSubscriptionPlansAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var coffeeItem = await _service.GetByIdAsync(id);
            if (coffeeItem == null)
            {
                return NotFound();
            }
            return Ok(coffeeItem);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateSubscriptionPlanDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Subcscription plan cannot be null");
            }
            var coffeeItem = _mapper.Map<SubscriptionPlan>(dto);

            var createdCoffeeItem = await _service.CreateAsync(coffeeItem);
            return Ok(_mapper.Map<SubscriptionPlanReponseDto>(createdCoffeeItem));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CreateSubscriptionPlanDto dto)
        {
            var existingSubscriptionPlan = await _service.GetByIdAsync(id);
            if (existingSubscriptionPlan == null)
            {
                return NotFound();
            }
            _mapper.Map(dto, existingSubscriptionPlan);

            var result = await _service.UpdateAsync(existingSubscriptionPlan);
            if (!result)
            {
                return BadRequest("Failed to update coffee");
            }

            return Ok(_mapper.Map<SubscriptionPlanReponseDto>(existingSubscriptionPlan));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();

        }
    }
}
