using System.Text.Json.Serialization;

namespace Core.DTOs.Response
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }
    }
}
