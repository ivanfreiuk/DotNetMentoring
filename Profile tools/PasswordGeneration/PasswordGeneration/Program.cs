using System;
using System.Security.Cryptography;

namespace PasswordGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            var salt = System.Text.Encoding.UTF8.GetBytes("testtesttesttest");

            for (int i = 0; i < 10000; i++)
            {
                GeneratePasswordHashUsingSalt($"welcome{i}", salt);
            }

            Console.ReadKey();
        }

        public static string GeneratePasswordHashUsingSaltLegacy(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }

        public static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {
            int iterationCount = 10000;
            using (var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterationCount))
            {
                byte[] hash = pbkdf2.GetBytes(20);
                var passwordHash = $"{Convert.ToBase64String(salt, 0, 16)}{Convert.ToBase64String(hash)}";
                return passwordHash;
            }            
        }
    }
}
