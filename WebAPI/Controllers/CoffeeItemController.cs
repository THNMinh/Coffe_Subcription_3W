using AutoMapper;
using Core.DTOs.CoffeeItemDTO;
using Core.DTOs.Request;
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
        private readonly ICloudinaryService _cloudinaryService;

        public CoffeeItemController(ICoffeeItemService service, IMapper mapper, 
            ICloudinaryService cloudinaryService)
        {
            _service = service;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
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
        [Consumes("multipart/form-data")] 
        public async Task<IActionResult> Create([FromForm] CoffeeItemRequestDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Coffee item cannot be null");
            }
            string imageUrl = null;
            if (dto.Image != null && dto.Image.Length > 0)
            {
                imageUrl = await _cloudinaryService.UploadImageAsync(dto.Image, "coffee_items");
            }

            var coffeeItem = _mapper.Map<CoffeeItem>(dto);
            coffeeItem.ImageUrl = imageUrl;

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

        [HttpPost("image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageRequestDTO request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file uploaded.");

            var url = await _cloudinaryService.UploadImageAsync(request.File, "coffee_sub/images");
            return Ok(new { imageUrl = url });
        }
    }
}
