using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class LoginDTO
    {
        [JsonPropertyName("token")]
        public string? AccessToken { get; set; }
    }
}
