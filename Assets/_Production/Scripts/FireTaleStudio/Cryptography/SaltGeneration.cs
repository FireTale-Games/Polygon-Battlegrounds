using System.Security.Cryptography;

namespace FTS.Cryptography
{
    internal sealed class SaltGeneration : ISaltGeneration
    {
        private static byte[] GenerateRandomSalt(int size)
        {
            using RNGCryptoServiceProvider rng = new();
            byte[] salt = new byte[size];
            rng.GetBytes(salt);
            return salt;
        }

        public byte[] GetSaltArray(int size) => GenerateRandomSalt(size);
    }

    internal interface ISaltGeneration
    {
        public byte[] GetSaltArray(int size);
    }
}