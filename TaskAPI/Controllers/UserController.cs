using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles ="Admin")]
        [HttpGet("All-Users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var user = await _userRepository.GetAllUsers();
                if (user == null)
                {
                    return NotFound("Nenhum usuário encontrado.");

                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");

            }

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var isDeleted = await _userRepository.DeleteUser(id);
                if (!isDeleted)
                {
                    return NotFound($"Usuário com ID {id} não encontrado.");
                }

                return NoContent();  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir usuário.");
                return StatusCode(500, "Ocorreu um erro interno.");
            }
        }
    }
}
