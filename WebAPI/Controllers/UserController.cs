using Core.DTOs.Request;
using Core.DTOs.Response;
using Core.DTOs;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Models;
using Mapster;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        #region Register

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequest)
        {
            if (_userService.IsUserExists(1, registerRequest.Email).GetAwaiter().GetResult())
            {
                return BadRequest(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "Email already exists"
                });
            }
            if (_userService.IsUserExists(2, registerRequest.Username).GetAwaiter().GetResult())
            {
                return BadRequest(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "Username already exists"
                });
            }
            await _userService.RegisterAsync(registerRequest);

            return StatusCode(StatusCodes.Status201Created, new ApiResponseDTO<object>
            {
                Success = true,
                Message = "User registered successfully"
            });
        }

        #endregion

        #region Get Users

        //[Authorize(Roles = "manager")]
        [HttpPost("getAll")]
        [ProducesResponseType(typeof(ApiResponseDTO<List<UserResponseDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers()
        {

            var users = await _userService.GetUsersAsync();

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
        [ProducesResponseType(typeof(ApiResponseDTO<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int userId)
        {
            if (userId == 0 || userId == null)
            {
                return BadRequest(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "Invalid user ID"
                });
            }
            var user = await _userService.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "User not found"
                });
            }

            return Ok(new ApiResponseDTO<UserDTO>
            {
                Success = true,
                Data = user
            });
        }
        #endregion

        #region Update

        [HttpPut("{id}")]
        //[Authorize(Roles = "staff")]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UserDTO requestDTO)
        {
            //User request = requestDTO.Adapt<User>();
            requestDTO.Id = id;
            requestDTO.UpdatedAt = DateTime.UtcNow;
            await _userService.UpdateAsync(requestDTO);
            return Ok(new { success = true });
        }
        #endregion

        #region Change Password

        [Authorize]
        [HttpPut("change-password")]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDTO request)
        {
            var user = await _userService.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return NotFound(new ApiResponseDTO<object>
                {
                    Success = true,
                    Message = "User not found."
                });
            }
            var isPasswordCorrect = await _userService.VerifyPassword(user, request.OldPassword);
            if (!isPasswordCorrect)
            {
                return BadRequest(new ApiResponseDTO<object>
                {
                    Success = true,
                    Message = "Old password is incorrect."
                });
            }

            bool isUpdated = await _userService.UpdatePassword(user, request.NewPassword);

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
        [HttpPut("change-isActive/{id}")]
        //[Authorize(Roles = $"{nameof(RoleEnum.Manager)}")]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeUserActive(int id, bool active)
        {
            var userDTO = await _userService.FindByIdAsync(id);
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

            var success = await _userService.ChangeUserActive(id, active);

            return Ok(new ApiResponseDTO<object> { Success = success });
        }
        #endregion
    }
}