using System;
using System.Security.Cryptography;

namespace JoggingTracker.Infrastructure.Provider.Security
{
    public class SecurityProvider : ISecurityProvider
    {
        private const int DerivedKeyLength = 24;
        private const int SaltLength = 24;

        private readonly int _iterationCount;

        public SecurityProvider(int iterationCount)
        {
            this._iterationCount = iterationCount;
        }

        public byte[] CalculatePasswordHash(string password)
        {
            var salt = this.ComputeRandomSalt();
            var passwordHash = this.ComputeHash(password, salt, this._iterationCount);
            var iterationCountArray = BitConverter.GetBytes(this._iterationCount);
            var combinedHash = new byte[SaltLength + DerivedKeyLength + iterationCountArray.Length];

            Buffer.BlockCopy(salt, 0, combinedHash, 0, SaltLength);
            Buffer.BlockCopy(passwordHash, 0, combinedHash, SaltLength, DerivedKeyLength);
            Buffer.BlockCopy(iterationCountArray, 0, combinedHash, salt.Length + passwordHash.Length, iterationCountArray.Length);

            return combinedHash;
        }

        public bool IsValidPassword(string passwordGuess, byte[] savedHash)
        {
            var salt = new byte[SaltLength];
            var actualPassword = new byte[DerivedKeyLength];
            var iterationCountLength = savedHash.Length - (salt.Length + actualPassword.Length);
            var iterationCountArray = new byte[iterationCountLength];

            Buffer.BlockCopy(savedHash, 0, salt, 0, SaltLength);
            Buffer.BlockCopy(savedHash, SaltLength, actualPassword, 0, actualPassword.Length);
            Buffer.BlockCopy(savedHash, salt.Length + actualPassword.Length, iterationCountArray, 0, iterationCountLength);

            var passwordGuessHash = this.ComputeHash(passwordGuess, salt, BitConverter.ToInt32(iterationCountArray, 0));

            return this.ConstantTimeComparison(passwordGuessHash, actualPassword);
        }

        private byte[] ComputeHash(string password, byte[] salt, int iterationCount)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterationCount))
            {
                return pbkdf2.GetBytes(DerivedKeyLength);
            }
        }

        private byte[] ComputeRandomSalt()
        {
            var cryptoService = RandomNumberGenerator.Create();
            var salt = new byte[SaltLength];

            cryptoService.GetBytes(salt);

            return salt;
        }

        private bool ConstantTimeComparison(byte[] passwordGuess, byte[] actualPassword)
        {
            var difference = (uint)passwordGuess.Length ^ (uint)actualPassword.Length;

            for (var i = 0; i < passwordGuess.Length && i < actualPassword.Length; i++)
            {
                difference |= (uint)(passwordGuess[i] ^ actualPassword[i]);
            }

            return difference == 0;
        }
    }
}
