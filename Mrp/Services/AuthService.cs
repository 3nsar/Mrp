using Mrp.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Mrp.Services
{
    public class AuthService
    {
        private readonly UserRepository userRepository;

        public AuthService(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        private static string HashPassword(string password, byte[] salt)
        {
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password, salt, 100_000, HashAlgorithmName.SHA256, 32);
            return Convert.ToBase64String(hash);
        }

        public bool Register(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            byte[] salt = RandomNumberGenerator.GetBytes(16);
            string hash = HashPassword(password, salt);
            return userRepository.CreateUser(username, hash, Convert.ToBase64String(salt));
        }


        public string? Login(string username, string password)
        {
            var user = userRepository.GetByUsername(username);
            if (user == null) return null;

            byte[] salt = Convert.FromBase64String(user.Salt);
            byte[] expected = Convert.FromBase64String(user.PasswordHash);
            byte[] actual = Convert.FromBase64String(HashPassword(password, salt));

            if (!CryptographicOperations.FixedTimeEquals(expected, actual))
                return null;

            string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            userRepository.SaveToken(user.ID, token);
            return token;
        }

        public bool ValidateToken(string token)
        {
            return userRepository.TokenExists(token);
        }
    }
}
