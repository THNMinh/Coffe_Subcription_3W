using CloudinaryDotNet.Actions;
using Core.DTOs;
using Core.DTOs.Request;
using Core.DTOs.Response;
using Core.Interfaces.Services;
using Core.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IPaginationService<UserResponseDTO> _paginationService;

        public UserController(IUserService service, IPaginationService<UserResponseDTO> paginationService)
        {
            _service = service;
            _paginationService = paginationService;
        }
        #region Register
        [HttpPost("")]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequest)
        {
            if (_service.IsUserExists(1, registerRequest.Email).GetAwaiter().GetResult())
            {
                return BadRequest(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "Email already exists"
                });
            }
            if (_service.IsUserExists(2, registerRequest.Username).GetAwaiter().GetResult())
            {
                return BadRequest(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "Username already exists"
                });
            }
            await _service.RegisterAsync(registerRequest);

            return StatusCode(StatusCodes.Status201Created, new ApiResponseDTO<object>
            {
                Success = true,
                Message = "User registered successfully"
            });
        }

        #endregion

        #region Search Users
        //[Authorize(Roles = "manager")]
        [HttpPost("search")]
        [ProducesResponseType(typeof(ApiResponseDTO<PagingResponseDTO<UserResponseDTO>>), StatusCodes.Status200OK)]
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

            return Ok(new ApiResponseDTO<PagingResponseDTO<UserResponseDTO>>
            {
                Success = true,
                Data = paginatedData
            });
        }

        #endregion

        #region Get Users
        //[Authorize(Roles = "manager")]
        [HttpGet("")]
        [ProducesResponseType(typeof(ApiResponseDTO<List<UserResponseDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers()
        {

            var users = await _service.GetUsersAsync();

            return Ok(new ApiResponseDTO<List<UserResponseDTO>>
            {
                Success = true,
                Data = users
            });
        }

        #endregion

        #region Get User
        [HttpGet("{Id}")]
        //[Authorize(Roles = "manager")]
        [ProducesResponseType(typeof(ApiResponseDTO<UserResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int Id)
        {
            if (Id == 0 || Id == null)
            {
                return BadRequest(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "Invalid user ID"
                });
            }
            var user = await _service.FindByIdAsync(Id);
            var dto = user.Adapt<UserResponseDTO>();
            dto.Role = user.RoleId switch
            {
                "1" => "Member",
                "2" => "Staff",
                "3" => "Manager",
                "4" => "Admin",
                "0" => "Unverified",
                _ => "Unknown"
            };

            if (user == null)
            {
                return NotFound(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "User not found"
                });
            }

            return Ok(new ApiResponseDTO<UserResponseDTO>
            {
                Success = true,
                Data = dto
            });
        }
        #endregion

        #region Update
        [HttpPut("{id}")]
        //[Authorize(Roles = "staff")]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UserRequestDTO requestDTO)
        {
            //User request = requestDTO.Adapt<User>();
            requestDTO.Id = id;
            if (requestDTO.CreatedAt.HasValue)
                requestDTO.CreatedAt = DateTime.SpecifyKind(requestDTO.CreatedAt.Value, DateTimeKind.Utc);
            requestDTO.UpdatedAt = DateTime.UtcNow;
            await _service.UpdateAsync(requestDTO);
            return Ok(new { success = true });
        }
        #endregion

        #region Change Password

        [Authorize]
        [HttpPut("password")]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDTO request)
        {
            var user = await _service.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return NotFound(new ApiResponseDTO<object>
                {
                    Success = true,
                    Message = "User not found."
                });
            }
            var isPasswordCorrect = await _service.VerifyPassword(user, request.OldPassword);
            if (!isPasswordCorrect)
            {
                return BadRequest(new ApiResponseDTO<object>
                {
                    Success = true,
                    Message = "Old password is incorrect."
                });
            }

            bool isUpdated = await _service.UpdatePassword(user, request.NewPassword);

            if (!isUpdated)
            {
                return StatusCode(500, new ApiResponseDTO<object>
                {
                    Success = true,
                    Message = "Error while updating"
                });
            }

            return Ok(new ApiResponseDTO<object>
            {
                Success = true,
                Message = "Password changed successfully"
            });
        }

        #endregion

        #region Deactive
        [HttpPut("is-active/{id}")]
        //[Authorize(Roles = $"{nameof(RoleEnum.Manager)}")]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeUserActive(int id, bool active)
        {
            var userDTO = await _service.FindByIdAsync(id);
            if (userDTO == null)
            {
                return NotFound(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "User not found"
                });
            }

            if (userDTO.IsActive == active)
            {
                return BadRequest(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = $"User is currently {(active ? "active" : "not active")}"
                });
            }

            var success = await _service.ChangeUserActive(id, active);

            return Ok(new ApiResponseDTO<object> { Success = success });
        }
        #endregion

        #region Deactive
        [HttpPut("role-member/{id}")]
        //[Authorize(Roles = $"{nameof(RoleEnum.Manager)}")]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> VerifyUser(int id, bool active)
        {
            var userDTO = await _service.FindByIdAsync(id);
            if (userDTO == null)
            {
                return NotFound(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "User not found"
                });
            }

            if (userDTO.IsActive == active)
            {
                return BadRequest(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "User is already a Member"
                });
            }

            var success = await _service.VerifyUser(id);

            return Ok(new ApiResponseDTO<object> { Success = success });
        }
        #endregion
    }
}
