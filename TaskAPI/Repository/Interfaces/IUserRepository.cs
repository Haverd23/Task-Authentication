using TaskAPI.Models;

namespace TaskAPI.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<UserModel> CreateUser(UserModel user);
        Task<UserModel> GetUserByEmail(UserModel user);
    }
}
