using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskAPI.DTOs.RefreshTokenDTO;
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
            else
            {
                return NotFound(new { message = "Email não encontrado" });
            }

            var token = await _tokenService.CreateToken(email); 
            var refreshToken = await _tokenService.GenerateRefreshToken(email.Id);

            return Ok(new 
            {
                Email = loginDTO.Email,
                Token = token,
                RefreshToken = refreshToken
            });
        }
        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDTO request)
        {
            var isValid = await _tokenService.ValidateRefreshToken(request.RefreshToken);
            if (!isValid)
            {
                return Unauthorized("Invalid refresh token.");
            }

            var user = await _userRepository.GetUserById(request.UserId);
            if (user == null)
            {
                return Unauthorized("Invalid user.");
            }

            var newAccessToken = await _tokenService.CreateToken(user); 
            var newRefreshToken = await _tokenService.GenerateRefreshToken(user.Id); 

            return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
        }
    }
}
