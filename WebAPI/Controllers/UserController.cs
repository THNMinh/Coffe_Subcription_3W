using Core.DTOs.Request;
using Core.DTOs.Response;
using Core.DTOs;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
