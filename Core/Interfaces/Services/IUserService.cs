using Core.DTOs;
using Core.DTOs.Request;
using Core.DTOs.Response;
using Core.Models;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<bool> IsUserExists(int type, string request);
        Task RegisterAsync(RegisterRequestDTO requestDTO);
        Task<User?> AuthenticateUserAsync(string email, string password);
        Task<UserDTO?> FindByEmailAsync(string email);
        Task<UserDTO?> FindByIdAsync(int id);
        Task<UserProfileResponseDTO?> FindProfileByIdAsync(int id);
        Task<bool> UpdatePassword(UserDTO user, string password);
        string GenerateRandomPassword();
        Task<bool> VerifyPassword(UserDTO user, string oldPassword);
        Task<List<UserResponseDTO>> GetUsersAsync();
        Task<bool> UpdateAsync(UserRequestDTO user);
        Task<bool> ChangeUserActive(int id, bool active);
        //Task<bool> ChangeUserStatusAsync(ChangeUserStatusRequestDTO requestDTO);
        //Task<bool> UpdateRoleAsync(UpdateRoleRequestDTO changeUserStatusDTO);
    }
}
