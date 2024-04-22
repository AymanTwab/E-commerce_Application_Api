using System.ComponentModel.DataAnnotations;

namespace Store.Service.Services.UserService.Dtos
{
    public class RegisterDto
    {
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*#?&])[A-Za-z\\d@$!%*#?&]{8,}$",ErrorMessage = "Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character:\r\n\r\n")]
        public string Password { get; set; }
    }
}
