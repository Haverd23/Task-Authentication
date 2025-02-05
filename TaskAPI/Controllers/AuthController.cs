using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskAPI.DTOs.UserDTO;
using TaskAPI.Models;
using TaskAPI.Repository.Interfaces;
using TaskAPI.Services;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly PasswordHasher _passwordHasher;
        private readonly TokenService _tokenService;
        private readonly IMapper _mapper; 
        private readonly IUserRepository _userRepository;

        public AuthController(PasswordHasher passwordHasher, TokenService tokenService, IMapper mapper, IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (loginDTO == null) return BadRequest("Dados nulos");
            var user = _mapper.Map<UserModel>(loginDTO);
            var email = await _userRepository.GetUserByEmail(user);
            if (email != null)
            {
                if (!_passwordHasher.VerifyPassword(loginDTO.Password, email.Password))
                {
                    return Unauthorized(new { message = "Senha incorreta" });
                }
            }
            else { return NotFound(new { message = "Email não encontrado" }); }
            var token = _tokenService.CreateToken(email);
            var tokenDTO = _mapper.Map<LoginDTO>(email);
            return Ok(new LoginDTO()
            {
                Email = loginDTO.Email,
                Password = loginDTO.Password,
                Token = token

            });
        }
    }
}
