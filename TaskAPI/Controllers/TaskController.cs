using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskAPI.DTOs.TaskDTO;
using TaskAPI.Models;
using TaskAPI.Repository.Interfaces;

namespace TaskAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITaskRepository _taskRepository;

        public TaskController(IMapper mapper, ITaskRepository taskRepository)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskDTO taskDTO)
        {
            try
            {
                if (taskDTO == null)
                {
                    return BadRequest("Tarefa não pode ser nula.");
                }

                var tarefa = _mapper.Map<TaskModel>(taskDTO);

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                tarefa.UserId = userId;

                var createdTarefa = await _taskRepository.Create(tarefa);

                return CreatedAtAction(nameof(CreateTask), new { id = createdTarefa.Id }, createdTarefa);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpGet("private")]
        public async Task<IActionResult> GetPrivateTasks()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized("Usuário não autenticado.");
                }

                var userId = int.Parse(userIdClaim);
                var user = await _taskRepository.GetByIdAsync(userId);

                if (user == null)
                {
                    return Unauthorized("Usuário não encontrado.");
                }

                var tarefas = await _taskRepository.GetPrivateTasks(user);

                if (tarefas == null || !tarefas.Any())
                {
                    return NotFound("Nenhuma tarefa privada encontrada.");
                }

                return Ok(tarefas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        
        }

        [HttpGet("public")]
        public async Task<IActionResult> GetPublicTasks()
        {
            try
            {
                var tarefas = await _taskRepository.GetPublicTasks();

                if (tarefas == null || !tarefas.Any())
                {
                    return NotFound("Nenhuma tarefa pública encontrada.");
                }

                return Ok(tarefas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var deletado = await _taskRepository.Delete(id);
                if (deletado == null)
                {
                    return BadRequest("Não foi possível deletar essa tarefa");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateTask(int id, UpdateTaskDTO updateTaskDto)
        {
            if (updateTaskDto == null)
            {
                return BadRequest("Dados da tarefa não podem ser nulos.");
            }

            var existingTask = await _taskRepository.GetById(id);
            if (existingTask == null)
            {
                return NotFound($"Tarefa com ID {id} não encontrada.");
            }

            var taskToUpdate = _mapper.Map<TaskModel>(updateTaskDto);
            taskToUpdate.Id = id;

            try
            {
                Console.WriteLine($"Atualizando tarefa com ID {taskToUpdate.Id}: {taskToUpdate.Name}");

                var updatedTask = await _taskRepository.Update(taskToUpdate);
                return Ok(updatedTask);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao atualizar a tarefa: " + ex.Message + " - " + ex.InnerException?.Message);
            }
        }
    }

}




