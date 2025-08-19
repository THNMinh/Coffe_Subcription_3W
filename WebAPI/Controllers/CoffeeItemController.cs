using AutoMapper;
using Core.DTOs.CoffeeItemDTO;
using Core.Interfaces.Services;
using Core.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeItemController : ControllerBase
    {
        private readonly ICoffeeItemService _service;
        private readonly IMapper _mapper;

         
        public CoffeeItemController(ICoffeeItemService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetAllCoffeeItemAsync();
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
        public async Task<IActionResult> Create([FromBody] CreateCoffeeItemDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Coffee item cannot be null");
            }
            var coffeeItem = _mapper.Map<CoffeeItem>(dto);

            var createdCoffeeItem = await _service.CreateAsync(coffeeItem);
            return Ok(createdCoffeeItem);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CreateCoffeeItemDto dto)
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

            return Ok(_mapper.Map<CoffeeItemResponseDto>(existingCoffee));
        }

        //[HttpPut("update")]
        //public async Task<IActionResult> Update([FromBody] CreateCoffeeItemDto dto)
        //{
        //    if (dto == null)
        //    {
        //        return BadRequest("Coffee item cannot be null");
        //    }
        //    var coffeeItem = _mapper.Map<CoffeeItem>(dto);

        //    var updated = await _service.UpdateAsync(coffeeItem);
        //    if (!updated)
        //    {
        //        return NotFound();
        //    }
        //    return Ok();
        //}

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

        [HttpGet("getSubAndCafeId")]
        public async Task<IActionResult> GetSubAndCafeId(int userId, int coffeeId)
        {
            var coffeeItem = await _service.GetCoffeeSubscriptionInfoAsync(userId, coffeeId);
            if (coffeeItem == null)
            {
                return NotFound();
            }
            return Ok(coffeeItem);
        }
    }
}
