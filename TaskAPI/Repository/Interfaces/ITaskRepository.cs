using TaskAPI.Models;

namespace TaskAPI.Repository.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskModel> Create(TaskModel task);
        Task<IEnumerable<TaskModel>> GetPrivateTasks(UserModel user);
        Task<UserModel> GetByIdAsync(int id);
        Task<TaskModel> Delete(int id);
        Task<TaskModel> Update(TaskModel task);

        Task<TaskModel> GetById(int id);


    }
}
