using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Core.DTOs.UserSubscriptionDTO;
using Core.Models;

namespace Core.DTOs.Response
{
    public class UserProfileResponseDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;


        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string Role { get; set; }

        public virtual UserSubscriptionProfileResponseDTO UserSubscriptions { get; set; } = new UserSubscriptionProfileResponseDTO();

    }
}
