using AutoMapper;
using Core.DTOs.CoffeeItemDTO;
using Core.DTOs.PlanCoffeeOptionDTO;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanCoffeeOptionController : ControllerBase
    {
        private readonly IPlanCoffeeOptionService _service;
        private readonly IMapper _mapper;
        
        public PlanCoffeeOptionController(IPlanCoffeeOptionService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetAllPlanCoffeeOptionAsync();
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
        public async Task<IActionResult> Create([FromBody] CreatePlanCoffeeOptionDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Coffee item cannot be null");
            }
            var coffeeItem = _mapper.Map<PlanCoffeeOption>(dto);

            var createdCoffeeItem = await _service.CreateAsync(coffeeItem);
            return Ok(_mapper.Map<PlanCoffeeOptionResponseDto>(createdCoffeeItem));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CreatePlanCoffeeOptionDto dto)
        {
            var existingCoffee = await _service.GetByIdAsync(id);
            if (existingCoffee == null)
            {
                return NotFound();
            }
            _mapper.Map(dto, existingCoffee);

            var result = await _service.UpdateAsync(existingCoffee);
            if (!result)
            {
                return BadRequest("Failed to update coffee");
            }

            return Ok(_mapper.Map<PlanCoffeeOptionResponseDto>(existingCoffee));
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
