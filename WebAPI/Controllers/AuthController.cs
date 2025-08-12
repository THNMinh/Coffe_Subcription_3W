using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces.Services;
using Core.DTOs;
using Core.DTOs.Response;
using Core.DTOs.Request;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AuthController(IJwtService jwtService, IUserService userService, IEmailService emailService)
        {
            _jwtService = jwtService;
            _userService = userService;
            _emailService = emailService;
        }

        #region Login

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponseDTO<LoginDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            var user = await _userService.AuthenticateUserAsync(loginRequest.Email, loginRequest.Password);
            if (user == null)
            {
                return Unauthorized(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "Invalid email or password."
                });
            }
            if (!user.IsActive)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "User is not permitted to log in. Account might be deactivated or restricted."
                });
            }

            var token = _jwtService.GenerateToken(user.Id.ToString(), user.Email, user.RoleId.ToString());
            return Ok(new ApiResponseDTO<LoginDTO>()
            {
                Success = true,
                Data = new LoginDTO() { AccessToken = token }
            });
        }

        #endregion

        #region Forgot Password

        [HttpPut("forgot-password")]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDTO request)
        {
            var user = await _userService.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "User with the provided email does not exist."
                });
            }

            // Generate a new password
            string newPassword = _userService.GenerateRandomPassword();

            // Parallelize the password update and email sending
            var updatePasswordTask = _userService.UpdatePassword(user, newPassword);
            var sendEmailTask = _emailService.SendEmailAsync(user.Email, "Your New Password", $"Your new password is: {newPassword}");

            // Await both tasks
            await Task.WhenAll(updatePasswordTask, sendEmailTask);

            // Send the password reset link to the user's email
            return Ok(new ApiResponseDTO<object>
            {
                Success = true,
                Message = "New password has been sent to your email."
            });
        }

        #endregion

        #region Get Current User

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseDTO<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCurrentUser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new ApiResponseDTO<object>
                {
                    Success = true,
                    Message = "User is not authenticated"
                });
            }

            // Extract user information from claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // User ID
            var email = User.FindFirst(ClaimTypes.Email)?.Value; // Email
            var role = User.FindFirst(ClaimTypes.Role)?.Value; // Role
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

            if (userId == null)
            {
                return Unauthorized(new ApiResponseDTO<object>
                {
                    Success = true,
                    Message = "User ID is not found in the claims."
                });
            }
            var userDTO = await _userService.FindByIdAsync(int.Parse(userId));
            userDTO.Role = role switch
            {
                "1" => "Member",
                "2" => "Staff",
                "3" => "Manager",
                "4" => "Admin",
                _ => "Unknown"
            };
            userDTO.Email = email ?? string.Empty;

            return Ok(new ApiResponseDTO<UserDTO>
            {
                Success = true,
                Data = userDTO
            });
            #endregion


        }
    }
}
