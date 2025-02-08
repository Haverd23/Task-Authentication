using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskAPI.Data;
using TaskAPI.Models;

namespace TaskAPI.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly appDbContext _context;

        public TokenService(IConfiguration configuration, appDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<string> CreateToken(UserModel user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value);

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.Name)
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = await GenerateRefreshToken(user.Id);

            return jwtTokenHandler.WriteToken(token);
        }

        public async Task<string> GenerateRefreshToken(int userId)
        {
            var refreshToken = new RefreshTokenModel
            {
                Token = Guid.NewGuid().ToString(),
                UserId = userId,
                ExpiryDate = DateTime.UtcNow.AddDays(2)
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        public async Task<bool> ValidateRefreshToken(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.ExpiryDate > DateTime.UtcNow);

            return token != null;
        }
    }
}
