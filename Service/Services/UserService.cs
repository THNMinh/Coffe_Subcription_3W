using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Core.Constants;
using Core.DTOs.Request;
using Core.DTOs;
using Core.Models;
using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Mapster;
using Core.Interfaces.Services;

namespace Service.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task RegisterAsync(RegisterRequestDTO requestDTO)
        {
            // Hash the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(requestDTO.Password);

            // Create the user and save to the database
            var newUser = new User
            {
                Email = requestDTO.Email,
                PasswordHash = hashedPassword,
                Username = requestDTO.Username,
                FullName = requestDTO.FullName,
                PhoneNumber = requestDTO.PhoneNumber,
                RoleId = requestDTO.RoleId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            await _userRepository.AddAsync(newUser);
        }

        public async Task<User?> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(user => user.Email == email);
            if (user == null)
            {
                // User with the given email doesn't exist
                return null;
            }
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isPasswordValid)
            {
                // Password is incorrect
                return null;
            }
            return user;
        }

        public async Task<bool> IsUserExists(int type, string request)
        {
            var user = await _userRepository.GetAsync(u =>
                (type == 1 && u.Email == request) || 
                (type == 2 && u.Username == request)
            );

            return user != null;
        }

        public async Task<UserDTO?> FindByEmailAsync(string email)
        {
            var user = await _userRepository.GetAsync(user => user.Email == email);
            if (user == null)
            {
                return null;
            }
            return user.Adapt<UserDTO>();
        }

        public async Task<bool> UpdatePassword(UserDTO userDTO, string password)
        {
            var user = await _userRepository.GetAsync(userDTO.UserId);
            if (user == null)
            {
                return false;
            }
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password); // Update with a hashed password
            user.UpdatedAt = DateTime.Now;
            return _userRepository.Update(user);
        }

        public string GenerateRandomPassword()
        {
            const int maxAsciiValue = 126; // '~' (tilde), the last printable ASCII character
            const int minAsciiValue = 33;  // '!' (exclamation mark), the first printable ASCII character
            int length = Consts.RANDOM_PASSWORD_LENGTH;

            var passwordChars = new char[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                for (int i = 0; i < length; i++)
                {
                    char generatedChar;
                    do
                    {
                        generatedChar = (char)(randomBytes[i] % (maxAsciiValue - minAsciiValue + 1) + minAsciiValue);
                    }
                    // Exclude problematic characters like '\' and '"'
                    while (generatedChar == '\\' || generatedChar == '"' || generatedChar == '\'');

                    passwordChars[i] = generatedChar;
                }
            }

            return new string(passwordChars);
        }

        public async Task<UserDTO?> FindByIdAsync(int id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return null;
            }
            return user.Adapt<UserDTO>();
        }

        public async Task<bool> VerifyPassword(UserDTO userDTO, string oldPassword)
        {
            var user = await _userRepository.GetAsync(userDTO.UserId);
            if (user == null)
            {
                return false;
            }
            return BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash);
        }

        //public async Task<(List<UserDTO>, int totalItems)> GetUsersAsync(SearchCondition searchCondition, PageInfoRequest pageInfo)
        //{
        //    // Start with a base filter that is always true
        //    Expression<Func<User, bool>> filter = u => true;

        //    // Apply filters dynamically
        //    if (!string.IsNullOrEmpty(searchCondition.Keyword))
        //    {
        //        string keyword = searchCondition.Keyword.ToLower();
        //        filter = AddFilter(filter, u =>
        //            (u.Name != null && u.Name.ToLower().Contains(keyword)) ||
        //            u.Email.ToLower().Contains(keyword));
        //    }
        //    if (!string.IsNullOrEmpty(searchCondition.Role))
        //    {
        //        var role = Enum<RoleEnum>.Parse(searchCondition.Role);
        //        filter = AddFilter(filter, u => u.Role == role);
        //    }


        //    filter = AddFilter(filter, u => u.Status == searchCondition.Status && u.IsDeleted == searchCondition.IsDeleted);

        //    var users = await _userRepository.GetWithPaginationAsync(pageInfo, filter);
        //    int totalItems = await _userRepository.CountAsync(filter);

        //    List<UserDTO> userDTOs = users.Select(user => user.Adapt<UserDTO>()).ToList();

        //    return (userDTOs, totalItems);
        //}

        //private Expression<Func<User, bool>> AddFilter(
        //    Expression<Func<User, bool>> existingFilter,
        //    Expression<Func<User, bool>> newFilter)
        //{
        //    var parameter = Expression.Parameter(typeof(User), "u");

        //    var combined = Expression.Lambda<Func<User, bool>>(
        //        Expression.AndAlso(
        //            Expression.Invoke(existingFilter, parameter),
        //            Expression.Invoke(newFilter, parameter)
        //        ),
        //        parameter
        //    );

        //    return combined;
        //}

        //public async Task<bool> ChangeUserStatusAsync(ChangeUserStatusRequestDTO requestDTO)
        //{
        //    // Retrieve the user by ID
        //    var user = await _userRepository.GetAsync(Guid.Parse(requestDTO.UserId));
        //    if (user == null) return false; // User not found

        //    // Check if the new status is different from the old status
        //    if (user.Status == requestDTO.Status.Value) return false;

        //    // Update the user status
        //    user.Status = requestDTO.Status ?? false;
        //    user.UpdatedDate = DateTime.Now;
        //    return _userRepository.Update(user);
        //}

        //public async Task<bool> DeleteUserAsync(int userId)
        //{
        //    var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (int.Parse(currentUserId) == userId) throw new BusinessException(ErrorDetails.CAN_NOT_DELETE_YOURSELF);
        //    var user = await _userRepository.GetAsync(userId);
        //    if (user == null) return false; // User not found

        //    user.IsDeleted = true;
        //    return _userRepository.Update(user);
        //}

        //public async Task<bool> UpdateRoleAsync(UpdateRoleRequestDTO changeUserStatusDTO)
        //{
        //    var user = await _userRepository.GetAsync(Guid.Parse(changeUserStatusDTO.UserId));
        //    if (user == null) return false; // User not found

        //    user.Role = Enum<RoleEnum>.Parse(changeUserStatusDTO.Role);
        //    user.UpdatedDate = DateTime.Now;
        //    return _userRepository.Update(user);
        //}
    }
}
