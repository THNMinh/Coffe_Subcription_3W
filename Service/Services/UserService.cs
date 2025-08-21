using System.Linq.Expressions;
using System.Security.Cryptography;
using Core.Constants;
using Core.DTOs;
using Core.DTOs.Request;
using Core.DTOs.Response;
using Core.DTOs.UserSubscriptionDTO;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Utils;
using Mapster;
using Microsoft.AspNetCore.Http;

namespace Service.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserSubcriptionRepository _userSubcriptionRepository;

        public UserService(IUserRepository userRepository, IJwtService jwtService,
            IHttpContextAccessor httpContextAccessor, IUserSubcriptionRepository userSubcriptionRepository)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
            _userSubcriptionRepository = userSubcriptionRepository;
        }

        public async Task RegisterAsync(RegisterRequestDTO requestDTO)
        {
            // Hash the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(requestDTO.Password);
            var maxId = await _userRepository.GetAllAsync();
            int newId = 1;
            if (maxId.Any())
            {
                newId = maxId.Max(u => u.Id) + 1;
            }
            // Create the user and save to the database
            var newUser = new User
            {
                Id = newId,
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
            await _userRepository.CreateAsync(newUser);
        }

        public async Task<User?> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
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
            var user = await _userRepository.GetAsync(user => user.Id == userDTO.Id);
            if (user == null)
            {
                return false;
            }
            user.UpdatedAt = DateTime.UtcNow;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return await _userRepository.UpdatePassword(user);
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
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            return user.Adapt<UserDTO>();
        }
        public async Task<UserProfileResponseDTO?> FindProfileByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;
            var userDTO = user.Adapt<UserProfileResponseDTO>();
            var userSub = await _userSubcriptionRepository.GetByUserIdAsync(user.Id);
            if (userSub != null)
            {
                userDTO.UserSubscriptions = userSub.Adapt<UserSubscriptionProfileResponseDTO>();
            }
            return userDTO;
        }

        public async Task<bool> VerifyPassword(UserDTO userDTO, string oldPassword)
        {
            var user = await _userRepository.GetByIdAsync(userDTO.Id);
            if (user == null)
            {
                return false;
            }
            return BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash);
        }

        public async Task<List<UserResponseDTO>> GetUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            //int totalItems = await _userRepository.CountAsync();
            List<UserResponseDTO> userDTOs = users.Select(user => user.Adapt<UserResponseDTO>()).ToList();
            return userDTOs;
        }

        public async Task<bool> UpdateAsync(UserRequestDTO userDTO)
        {
            var user = await _userRepository.GetAsync(u => u.Id == userDTO.Id);
            if (user == null) return false;

            if (!string.IsNullOrEmpty(userDTO.Username))
                user.Username = userDTO.Username;

            if (!string.IsNullOrEmpty(userDTO.PasswordHash))
                user.PasswordHash = userDTO.PasswordHash;

            if (!string.IsNullOrEmpty(userDTO.FullName))
                user.FullName = userDTO.FullName;

            if (!string.IsNullOrEmpty(userDTO.Email))
                user.Email = userDTO.Email;

            if (!string.IsNullOrEmpty(userDTO.PhoneNumber))
                user.PhoneNumber = userDTO.PhoneNumber;

            user.CreatedAt = DateTime.SpecifyKind(user.CreatedAt, DateTimeKind.Utc);


            user.UpdatedAt = DateTime.UtcNow; ;

            if (userDTO.RoleId.HasValue)
                user.RoleId = userDTO.RoleId;

            if (userDTO.IsActive.HasValue)
                user.IsActive = userDTO.IsActive.Value;

            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> ChangeUserActive(int id, bool active)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            user.IsActive = active;
            user.UpdatedAt = DateTime.UtcNow;
            user.CreatedAt = DateTime.SpecifyKind(user.CreatedAt, DateTimeKind.Utc);
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<(IEnumerable<UserResponseDTO>, int totalItems)> GetAllWithSearch (Search searchCondition, PageInfoRequestDTO pageInfo)
        {
            // Start with a base filter that is always true
            Expression<Func<User, bool>> filter = u => true;
            // Apply filters dynamically
            if (!string.IsNullOrEmpty(searchCondition.Keyword))
            {
                string keyword = searchCondition.Keyword.ToLower();
                filter = ExpressionUtils.AddFilter(filter, c =>
                    c.Username.ToLower().Contains(keyword) ||
                    (c.FullName != null && c.FullName.ToLower().Contains(keyword) ||
                    (c.Email != null && c.Email.ToLower().Contains(keyword))));
            }
            filter = ExpressionUtils.AddFilter(filter, c => c.IsDelete == searchCondition.IsDelete);

            var items = await _userRepository.GetWithPaginationAsync(pageInfo, filter);
            int totalItems = await _userRepository.CountAsync(filter);

            List<UserResponseDTO> itemDTOs = items.Select(u =>
            {
                var dto = u.Adapt<UserResponseDTO>();
                dto.Role = u.RoleId switch
                {
                    1 => "Member",
                    2 => "Staff",
                    3 => "Manager",
                    4 => "Admin",
                    0 => "Unverified"
                };
                return dto;
            }).ToList();

            return (itemDTOs, totalItems);
        }

        public async Task<bool> VerifyUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;
            user.RoleId = 1;
            user.UpdatedAt = DateTime.UtcNow;
            user.CreatedAt = DateTime.SpecifyKind(user.CreatedAt, DateTimeKind.Utc);
            return await _userRepository.UpdateAsync(user);
        }
    }
}
