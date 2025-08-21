using Core.DTOs.Request;
using Core.DTOs.Response;
using Core.DTOs;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using Core.DTOs.CoffeeItemDTO;
using Service.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly IPaginationService<CategoryDTO> _paginationService;


        public CategoryController(ICategoryService service, IPaginationService<CategoryDTO> paginationService)
        {
            _service = service;
            _paginationService = paginationService;
        }

        #region Get All
        //[Authorize(Roles = "manager")]
        [HttpGet("")]
        [ProducesResponseType(typeof(ApiResponseDTO<List<CategoryResponseDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories()
        {

            var categories = await _service.GetAllCategoryAsync();
            var categoryDTOs = categories.Adapt<List<CategoryResponseDTO>>();
            return Ok(new ApiResponseDTO<List<CategoryResponseDTO>>
            {
                Success = true,
                Data = categoryDTOs
            });
        }
        #endregion 

        #region Get

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseDTO<CategoryResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _service.GetByIdAsync(id);
            var categoryDTO = category.Adapt<CategoryResponseDTO>();
            if (category == null)
            {
                return NotFound(new ApiResponseDTO<object>() { Success = false, Message = "Category not found" });
            }
            return Ok(new ApiResponseDTO<CategoryResponseDTO>() { Success = true, Data = categoryDTO });
        }

        #endregion

        #region Create

        [HttpPost("")]
        //[Authorize(Roles = "staff")]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CategoryRequestDTO requestDTO)
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
            var request = requestDTO.Adapt<Category>();
            var isSuccess = await _service.CreateAsync(request);
            if (isSuccess.Equals(null))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "Failed to create Category"
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
        public async Task<IActionResult> Update(int id, [FromBody] CategoryRequestDTO requestDTO)
        {
            var request = requestDTO.Adapt<Category>();
            request.CategoryId = id;
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
            var category = await _service.GetByIdAsync(id);
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
                    Message = "Category not found"
                });
            }

            return Ok(new ApiResponseDTO<object> { Success = true });
        }

        #endregion

        #region Search

        [HttpPost("search")]
        [ProducesResponseType(typeof(ApiResponseDTO<PagingResponseDTO<CategoryDTO>>), StatusCodes.Status200OK)]
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

            var (data, totalItems) = await _service.GetAllCategoriesAsync(requestDTO.SearchCondition, requestDTO.PageInfo);
            var paginatedData = _paginationService.GetPagedData(totalItems, data, requestDTO.PageInfo);

            return Ok(new ApiResponseDTO<PagingResponseDTO<CategoryDTO>>
            {
                Success = true,
                Data = paginatedData
            });
        }

        #endregion 
    }
}
