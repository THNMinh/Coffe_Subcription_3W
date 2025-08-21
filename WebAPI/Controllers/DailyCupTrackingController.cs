using Core.DTOs;
using Core.DTOs.CoffeeItemDTO;
using Core.DTOs.Request;
using Core.DTOs.Response;
using Core.Interfaces.Services;
using Core.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyCupTrackingController : ControllerBase
    {
        private readonly IDailyCupTrackingService _service;
        private readonly IPaginationService<DailyCupTrackingDTO> _paginationService;


        public DailyCupTrackingController(IDailyCupTrackingService service, 
            IPaginationService<DailyCupTrackingDTO> paginationService)
        {
            _service = service;
            _paginationService = paginationService;
        }
        #region Get All

        [HttpPost("search")]
        [ProducesResponseType(typeof(ApiResponseDTO<PagingResponseDTO<DailyCupTrackingDTO>>), StatusCodes.Status200OK)]
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

            var (data, totalItems) = await _service.GetAllWithSearch(requestDTO.SearchCondition, requestDTO.PageInfo);
            var paginatedData = _paginationService.GetPagedData(totalItems, data, requestDTO.PageInfo);

            return Ok(new ApiResponseDTO<PagingResponseDTO<DailyCupTrackingDTO>>
            {
                Success = true,
                Data = paginatedData
            });
        }

        #endregion

        #region Get

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseDTO<DailyCupTrackingDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var tracking = await _service.GetByIdAsync(id);
            if (tracking == null)
            {
                return NotFound(new ApiResponseDTO<object>() { Success = false, Message = "Daily Cup Tracking not found" });
            }
            return Ok(new ApiResponseDTO<DailyCupTrackingDTO>() { Success = true, Data = tracking });
        }

        #endregion

        #region Create

        [HttpPost("")]
        //[Authorize(Roles = "staff")]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] DailyCupTrackingRequestDTO requestDTO)
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
            DailyCupTracking request = requestDTO.Adapt<DailyCupTracking>();
            var isSuccess = await _service.CreateAsync(request);
            if (isSuccess.Equals(null))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "Failed to create Daily Cup Tracking"
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
        public async Task<IActionResult> Update(int id, [FromBody] DailyCupTrackingRequestDTO requestDTO)
        {
            DailyCupTracking request = requestDTO.Adapt<DailyCupTracking>();
            request.TrackingId = id;
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
            var category = await _service.GetByIdAsyncForDelete(id);
            if (category == null)
            {
                return NotFound();
            }
            category.IsDelete = false;
            var success = await _service.UpdateAsync(category);

            //var success = await _service.DeleteAsync(id);

            if (!success)
            {
                return NotFound(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "Daily cup tracking not found"
                });
            }

            return Ok(new ApiResponseDTO<object> { Success = true });
        }

        #endregion
    }
}
