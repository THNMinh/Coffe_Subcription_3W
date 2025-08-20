using System.Text.Json.Serialization;

namespace Core.DTOs.Request
{
    public class UserRequestDTO
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string? Username { get; set; }

        public string? PasswordHash { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        public int? RoleId { get; set; }

        public bool? IsActive { get; set; }
    }
}
