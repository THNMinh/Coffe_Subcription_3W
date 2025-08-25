using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.DTOs;
using Core.DTOs.Request;
using Core.DTOs.Response;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AuthController(IJwtService jwtService, IGoogleAuthService googleAuthService,
            IUserService userService, IEmailService emailService)
        {
            _jwtService = jwtService;
            _googleAuthService = googleAuthService;
            _userService = userService;
            _emailService = emailService;
            ;
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

        //#region Google Login
        //[HttpPost("google-login")]
        //[ProducesResponseType(typeof(ApiResponseDTO<LoginDTO>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status401Unauthorized)]
        //public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDTO request)
        //{
        //    string idToken = request?.Token;
        //    if (request == null || string.IsNullOrEmpty(idToken))
        //        return BadRequest(new ApiResponseDTO<object>
        //        {
        //            Success = false,
        //            Message = "No Token provided"
        //        });

        //    try
        //    {
        //        var payload = await _googleAuthService.VerifyGoogleTokenAsync(idToken);

        //        var email = payload.Email;
        //        var name = payload.Name;
        //        var googleId = payload.Subject;

        //        // TODO: Check or create user in your DB

        //        var appToken = _jwtService.GenerateToken(email, googleId, "1");

        //        return Ok(new { token = appToken, email, name });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Unauthorized(new ApiResponseDTO<object>
        //        {
        //            Success = false,
        //            Message = "Invalid Google Token."
        //        });
        //    }
        //}
        //#endregion
        #region Google Login
        [HttpPost("google-login")]
        [ProducesResponseType(typeof(ApiResponseDTO<LoginDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDTO<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDTO request)
        {
            string idToken = request?.idToken;
            if (string.IsNullOrEmpty(idToken))
                return BadRequest(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "No Token provided"
                });

            try
            {
                var payload = await _googleAuthService.VerifyGoogleTokenAsync(idToken);

                var email = payload.Email;
                var name = payload.Name;
                var googleId = payload.Subject;

                // 1. Check user exists in DB
                var user = await _userService.FindByEmailAsync(email);

                if (user == null)
                {
                    // Generate a new password
                    string newPassword = _userService.GenerateRandomPassword();                
                    var newUser = new RegisterRequestDTO
                    {
                        Email = email,
                        Username = email.Split('@')[0],
                        FullName = name,
                        Password = newPassword,
                        RoleId = 1,
                    };

                    await _userService.RegisterAsync(newUser);
                }

                if (!user.IsActive)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ApiResponseDTO<object>
                    {
                        Success = false,
                        Message = "User is not permitted to log in. Account might be deactivated or restricted."
                    });
                }

                // 3. Generate JWT token
                var appToken = _jwtService.GenerateToken(user.Id.ToString(), user.Email, user.RoleId.ToString());

                return Ok(new ApiResponseDTO<LoginDTO>()
                {
                    Success = true,
                    Data = new LoginDTO { AccessToken = appToken }
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "Invalid Google Token."
                });
            }
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
        [HttpGet("current-logged-user")]
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
            var userDTO = await _userService.FindProfileByIdAsync(int.Parse(userId));
            userDTO.Role = role switch
            {
                "1" => "Member",
                "2" => "Staff",
                "3" => "Manager",
                "4" => "Admin",
                _ => "Unknown"
            };
            userDTO.Email = email ?? string.Empty;
            return Ok(new ApiResponseDTO<UserProfileResponseDTO>
            {
                Success = true,
                Data = userDTO
            });
        }
        #endregion

        #region Get Current User With Token
        [HttpPost("current-logged-user-with-token")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponseDTO<UserProfileResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCurrentUser([FromBody] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "JWT token is missing."
                });
            }
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken;
            try
            {
                jwtToken = handler.ReadJwtToken(token); 
            }
            catch
            {
                return Unauthorized(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "Invalid JWT token."
                });
            }

            // Extract claims
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "User ID is missing in token."
                });
            }
            var userDTO = await _userService.FindProfileByIdAsync(int.Parse(userIdClaim));
            if (userDTO == null)
            {
                return NotFound(new ApiResponseDTO<object>
                {
                    Success = false,
                    Message = "User not found."
                });
            }

            // Map claims to DTO
            userDTO.Email = emailClaim ?? string.Empty;
            userDTO.Role = roleClaim switch
            {
                "1" => "Member",
                "2" => "Staff",
                "3" => "Manager",
                "4" => "Admin",
                _ => "Unknown"
            };
            return Ok(new ApiResponseDTO<UserProfileResponseDTO>
            {
                Success = true,
                Data = userDTO
            });
        }
        #endregion
    }
}
