using Microsoft.EntityFrameworkCore;
using TaskAPI.Data;
using TaskAPI.Models;
using TaskAPI.Repository.Interfaces;

namespace TaskAPI.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly appDbContext _appDbContext;

        public TaskRepository(appDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<TaskModel> Create(TaskModel task)
        {
            task.DataCriacao = DateTime.UtcNow;
            _appDbContext.Add(task);
            await _appDbContext.SaveChangesAsync();
            return task;
            
        }
        public async Task<IEnumerable<TaskModel>> GetPrivateTasks(UserModel user)
        {
            if(user == null) throw new NullReferenceException("Usuário com este email já existe.");
            var query = _appDbContext.Tasks.AsQueryable();

            if (user.Role == "Admin")
            {
                query = query.Where(x => x.UserId == user.Id || x.IsPublic == true);
            }
            else
            {
                query = query.Where(x => x.UserId == user.Id);
            }

            return await query.ToListAsync();
        }
        public async Task<UserModel> GetByIdAsync(int id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                await _appDbContext.Users.FindAsync(id);
                return user;
            }
            return null;
            
        }
        public async Task<TaskModel> Delete(int id)
        {
            var deletado = await _appDbContext.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if(deletado != null)
            {
                 _appDbContext.Remove(deletado);
                await _appDbContext.SaveChangesAsync();
                return deletado;
            }
            throw new InvalidOperationException("Não foi possível deletar essa tarefa");
        }

        public async Task<TaskModel> Update(TaskModel task)
        {
            var existingTask = await _appDbContext.Tasks.FindAsync(task.Id);
            if (existingTask == null)
            {
                throw new InvalidOperationException($"Tarefa com ID {task.Id} não encontrada.");
            }

            existingTask.Name = task.Name;
            existingTask.Description = task.Description;
            existingTask.IsPublic = task.IsPublic;

            await _appDbContext.SaveChangesAsync();
            return existingTask;
        }

        public async Task<TaskModel> GetById(int id)
        {
            return await _appDbContext.Tasks.FindAsync(id);
        }
    }
}
