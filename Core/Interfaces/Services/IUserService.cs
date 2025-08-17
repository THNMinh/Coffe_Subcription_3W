using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;
using Core.DTOs.Request;
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
        Task<bool> UpdatePassword(UserDTO user, string password);
        string GenerateRandomPassword();
        Task<bool> VerifyPassword(UserDTO user, string oldPassword);
        //Task<(List<UserDTO>, int totalItems)> GetUsersAsync(SearchCondition searchCondition, PageInfoRequest pageInfo);
        //Task<bool> ChangeUserStatusAsync(ChangeUserStatusRequestDTO requestDTO);
        //Task<bool> DeleteUserAsync(int userId);
        //Task<bool> UpdateRoleAsync(UpdateRoleRequestDTO changeUserStatusDTO);
    }
}
