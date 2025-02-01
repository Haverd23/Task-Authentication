using Microsoft.EntityFrameworkCore;
using TaskAPI.Data;
using TaskAPI.Models;
using TaskAPI.Repository.Interfaces;

namespace TaskAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly appDbContext _appDbContext;

        public UserRepository(appDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<UserModel> CreateUser(UserModel user)
        {
          
            
            var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null) throw new InvalidOperationException("Usuário com este email já existe.");


            user.Role = "User";

           _appDbContext.Add(user);
           await _appDbContext.SaveChangesAsync();
           return user;


        }
    }
}
