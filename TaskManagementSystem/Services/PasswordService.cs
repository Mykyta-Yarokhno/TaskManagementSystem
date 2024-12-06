using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace TaskManagementSystem.Services
{
    public class PasswordService
    {
        private const int MinPasswordLength = 8;

        public bool IsPasswordComplexEnough(string password)
        {
            if (password.Length < MinPasswordLength)
            {
                return false; 
            }
            if (!password.Any(ch => !Char.IsLetterOrDigit(ch)))
            {
                return false;
            }

            return true;
        }

        public string HashPassword(string password) 
        {
            if (!IsPasswordComplexEnough(password))
            {
                throw new ArgumentException("Password does not meet the complexity requirements.");
            }

            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,              
                salt: salt,                      
                prf: KeyDerivationPrf.HMACSHA256, 
                iterationCount: 10000,           
                numBytesRequested: 256 / 8));    

            return Convert.ToBase64String(salt) + ":" + hashedPassword; 
        }

        public bool VerifyPassword(string hashedPasswordWithSalt, string password)
        {
            var parts = hashedPasswordWithSalt.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = parts[1];

            string hashOfInput = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return storedHash == hashOfInput;
        }
    }
}
