using System.ComponentModel.DataAnnotations;

namespace TaskAPI.DTOs.UserDTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
