using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Request
{
    public class ChangePasswordRequestDTO
    {
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Old Password is required")]
        public string OldPassword { get; set; } = null!;
        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; } = null!;
    }
}
