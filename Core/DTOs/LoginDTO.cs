using System.Text.Json.Serialization;

namespace Core.DTOs
{
    public class LoginDTO
    {
        [JsonPropertyName("token")]
        public string? AccessToken { get; set; }
    }
}
