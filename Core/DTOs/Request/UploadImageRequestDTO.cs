using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Core.DTOs.Request
{
    public class UploadImageRequestDTO
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
