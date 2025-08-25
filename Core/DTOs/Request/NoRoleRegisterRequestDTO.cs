using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Constants.Consts;

namespace Core.DTOs.Request
{
    public class NoRoleRegisterRequestDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = null!;

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is required")]
        [MinLength(PASSWORD_MIN_LENGTH, ErrorMessage = "{0} must be at least {1} characters")]
        public string Password { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }
    }
}
