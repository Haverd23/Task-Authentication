using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskAPI.DTOs.UserDTO;
using TaskAPI.Models;
using TaskAPI.Repository.Interfaces;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IMapper mapper, IUserRepository userRepository, ILogger<UserController> logger)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (registerDTO == null || !ModelState.IsValid)
            {
                return BadRequest("Dados inválidos ou modelo incompleto.");
            }

            try
            {
                var user = _mapper.Map<UserModel>(registerDTO);
                var createUser = await _userRepository.CreateUser(user);
                var createdUser = _mapper.Map<RegisterDTO>(createUser);

                return CreatedAtAction(nameof(Register), new { name = createdUser.Name }, createdUser);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar usuário.");
                return StatusCode(500, "Ocorreu um erro interno.");
            }
        }
    }
}
