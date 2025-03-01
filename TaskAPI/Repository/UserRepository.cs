using Microsoft.EntityFrameworkCore;
using TaskAPI.Data;
using TaskAPI.Models;
using TaskAPI.Repository.Interfaces;
using TaskAPI.Services;

namespace TaskAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly appDbContext _appDbContext;
        private readonly PasswordHasher _passwordHasher;

        public UserRepository(appDbContext appDbContext, PasswordHasher passwordHasher)
        {
            _appDbContext = appDbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserModel> CreateUser(UserModel user)
        {
          
            
            var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null) throw new InvalidOperationException("Usuário com este email já existe.");

	    if(user.Email == "admin@gmail.com"){
		user.Role = "Admin";
	    }
	    else{
            user.Role = "User";
	    }
            user.Password = _passwordHasher.HashPassword(user.Password);
            
           _appDbContext.Add(user);
           await _appDbContext.SaveChangesAsync();
           return user;


        }
        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            var email = await _appDbContext.Users.ToListAsync();  
            return email;
        }
        public async Task<UserModel> GetUserByEmail(UserModel user)
        {
            var email = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            return email;
        }
        public async Task<UserModel> GetUserById(int userId)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;  
            }

            _appDbContext.Users.Remove(user);
            await _appDbContext.SaveChangesAsync();
            return true;  
        }


    }
}
