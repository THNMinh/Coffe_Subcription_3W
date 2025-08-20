using Core.DTOs.UserSubscriptionDTO;

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
