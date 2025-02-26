using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskAPI.Data;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly appDbContext  _context;

        public StatisticsController(appDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetStatistics")]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                var userCount = await _context.Users.CountAsync();

                var totalTasks = await _context.Tasks.CountAsync();

                var publicTasks = await _context.Tasks.CountAsync(t => t.IsPublic == true);

                var privateTasks = await _context.Tasks.CountAsync(t => t.IsPublic == false);

                return Ok(new
                {
                    UserCount = userCount,
                    TotalTasks = totalTasks,
                    PublicTasks = publicTasks,
                    PrivateTasks = privateTasks
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter as estatísticas: " + ex.Message);
            }
        }
    }
}

