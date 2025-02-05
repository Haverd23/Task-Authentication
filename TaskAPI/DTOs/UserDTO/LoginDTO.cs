namespace TaskAPI.DTOs.UserDTO
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string? Token { get; set; }
    }
}
