using AutoMapper;
using Core.DTOs;
using Core.DTOs.CoffeeItemDTO;
using Core.DTOs.Request;
using Core.DTOs.Response;
using Core.Interfaces.Services;
using Core.Models;
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
        private readonly IPaginationService<CoffeeItemDTO> _paginationService;
        private readonly ICloudinaryService _cloudinaryService;

        public CoffeeItemController(ICoffeeItemService service, IMapper mapper,
            ICloudinaryService cloudinaryService, IPaginationService<CoffeeItemDTO> paginationService)
        {
            _service = service;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
            _paginationService = paginationService;
        }

        [HttpPost("search")]
        [ProducesResponseType(typeof(ApiResponseDTO<PagingResponseDTO<CoffeeItemDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromBody] GetAllRequestDTO requestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "Invalid input",
                    Errors = ModelState.Keys.Select(key => new ValidationErrorDTO
                    {
                        Field = key,
                        Message = ModelState[key]?.Errors.Select(e => e.ErrorMessage).ToList()
                    }).ToList()
                });
            }

            var (data, totalItems) = await _service.GetAllCoffeeItemsAsync(requestDTO.SearchCondition, requestDTO.PageInfo);
            var paginatedData = _paginationService.GetPagedData(totalItems, data, requestDTO.PageInfo);

            return Ok(new ApiResponseDTO<PagingResponseDTO<CoffeeItemDTO>>
            {
                Success = true,
                Data = paginatedData
            });
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

        [HttpPost("")]
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
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Update(int id, [FromForm] UpdateCoffeeItemDTO dto)
        {
            var existingCoffee = await _service.GetByIdAsync(id);
            if (existingCoffee == null)
            {
                return NotFound();
            }
            bool isUpdated = false;
            if (!string.IsNullOrWhiteSpace(dto.CoffeeName))
            {
                existingCoffee.CoffeeName = dto.CoffeeName;
                isUpdated = true;
            }
            if (!string.IsNullOrWhiteSpace(dto.Description))
            {
                existingCoffee.Description = dto.Description;
                isUpdated = true;
            }
            if (!string.IsNullOrWhiteSpace(dto.Code))
            {
                existingCoffee.Code = dto.Code;
                isUpdated = true;
            }
            if (dto.IsActive.HasValue)
            {
                existingCoffee.IsActive = dto.IsActive.Value;
                isUpdated = true;
            }
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(dto.Image, "coffee_items");
                existingCoffee.ImageUrl = imageUrl;
                isUpdated = true;
            }
            if (!isUpdated)
            {
                return BadRequest("No change have been made"); 
            }
            var result = await _service.UpdateAsync(existingCoffee);
            if (!result)
            {
                return BadRequest("Failed to update coffee");
            }

            return Ok(_mapper.Map<CoffeeItemResponseDto>(existingCoffee));
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

        [HttpPost("image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageRequestDTO request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file uploaded.");

            var url = await _cloudinaryService.UploadImageAsync(request.File, "coffee_sub/images");
            return Ok(new { imageUrl = url });
        }


        [HttpPost("qrcode")]
        public async Task<IActionResult> ValidateCoffee([FromBody] ValidateCoffeeRequest request)
        {
            var validationResult = await _service.ValidateCoffeeRedemptionAsync(
                request.UserId,
                request.CoffeeId);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = validationResult.Message
                });
            }

            return Ok(new
            {
                success = true,
                message = validationResult.Message,
                subscriptionId = validationResult.SubscriptionId,
                coffeeCode = validationResult.CoffeCode,
                userId = request.UserId,
            });
        }

        public class ValidateCoffeeRequest
        {
            public int UserId { get; set; }
            public int CoffeeId { get; set; }
        }

        //[HttpGet("getSubAndCafeId")]
        //public async Task<IActionResult> GetSubAndCafeId(int userId, int coffeeId)
        //{
        //    var coffeeItem = await _service.GetCoffeeSubscriptionInfoAsync(userId, coffeeId);
        //    if (coffeeItem == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(coffeeItem);
        //}
    }
}
