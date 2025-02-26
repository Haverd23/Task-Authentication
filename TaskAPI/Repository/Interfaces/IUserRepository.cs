using TaskAPI.Models;

namespace TaskAPI.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<UserModel> CreateUser(UserModel user);
        Task<UserModel> GetUserByEmail(UserModel user);
        Task<UserModel> GetUserById(int userId);
        Task<IEnumerable<UserModel>> GetAllUsers();
        Task<bool> DeleteUser(int userId);  
    }
}
