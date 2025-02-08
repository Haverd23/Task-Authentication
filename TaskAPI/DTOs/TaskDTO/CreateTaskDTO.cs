using System.ComponentModel.DataAnnotations;

namespace TaskAPI.DTOs.TaskDTO
{
    public class CreateTaskDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "O nome da tarefa deve ter no máximo 100 caracteres.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string Description { get; set; }

        public bool IsPublic { get; set; }
    }
}
