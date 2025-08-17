using Core.DTOs.Response;
using Core.DTOs;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using Core.DTOs.Request;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionTimeWindowController : ControllerBase
    {
        private readonly ISubscriptionTimeWindowService _service;

        public SubscriptionTimeWindowController(ISubscriptionTimeWindowService service)
        {
            _service = service;
        }
        #region Get All

        [HttpPost("getAll")]
        [ProducesResponseType(typeof(ApiResponseDTO<SubscriptionTimeWindowDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
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

            var times = await _service.GetAllSubscriptionTimeWindowsAsync();

            return Ok(new ApiResponseDTO<List<SubscriptionTimeWindowDTO>>
            {
                Success = true,
                Data = times
            });
        }

        #endregion

        #region Get

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseDTO<SubscriptionTimeWindowDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var time = await _service.GetByIdAsync(id);
            if (time == null)
            {
                return NotFound(new ApiResponseDTO<object>() { Success = false, Message = "Time not found" });
            }
            return Ok(new ApiResponseDTO<SubscriptionTimeWindowDTO>() { Success = true, Data = time });
        }

        #endregion

        #region Create

        [HttpPost("create")]
        //[Authorize(Roles = "staff")]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] SubscriptionTimeWindowRequestDTO requestDTO)
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
            SubscriptionTimeWindow request = requestDTO.Adapt<SubscriptionTimeWindow>();
            var isSuccess = await _service.CreateAsync(request);
            if (!isSuccess.Equals(null))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "Failed to create Subscription Time Window"
                });
            }

            return Ok(new { success = true });
        }

        #endregion

        #region Update

        [HttpPut("{id}")]
        //[Authorize(Roles = "staff")]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] SubscriptionTimeWindowRequestDTO requestDTO)
        {
            SubscriptionTimeWindow request = requestDTO.Adapt<SubscriptionTimeWindow>();
            await _service.UpdateAsync(request);

            return Ok(new { success = true });
        }

        #endregion

        #region Delete

        [HttpDelete("{id}")]
        //[Authorize(Roles = $"{nameof(RoleEnum.Manager)}")]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);

            if (!success)
            {
                return NotFound(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "Subscription Time Window not found"
                });
            }

            return Ok(new ApiResponseDTO<object> { Success = true }); // Successfully deleted
        }

        #endregion
    }
}
