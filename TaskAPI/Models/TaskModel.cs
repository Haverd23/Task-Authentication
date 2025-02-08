using System.ComponentModel.DataAnnotations;

namespace TaskAPI.Models
{
    public class TaskModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime DataCriacao { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }
    }
}
