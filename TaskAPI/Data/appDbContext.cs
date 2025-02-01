using Microsoft.EntityFrameworkCore;
using TaskAPI.Models;

namespace TaskAPI.Data
{
    public class appDbContext : DbContext
    {
        public appDbContext(DbContextOptions<appDbContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
    }
}
