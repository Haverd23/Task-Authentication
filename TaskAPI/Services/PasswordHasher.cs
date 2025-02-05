using System.Security.Cryptography;
using System.Text;

namespace TaskAPI.Services
{
    public class PasswordHasher
    {
        public string HashPassword(string password)
        {
            using (var hmac = new HMACSHA256())
            {
                var salt = hmac.Key;

                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));


                return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
            }
        }


        public bool VerifyPassword(string password, string storedHash)
        {

            var parts = storedHash.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            var storedPasswordHash = Convert.FromBase64String(parts[1]);

 
            using (var hmac = new HMACSHA256(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

               
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedPasswordHash[i])
                    {
                        return false; 
                    }
                }
            }

            return true; 
        }
    }
}

