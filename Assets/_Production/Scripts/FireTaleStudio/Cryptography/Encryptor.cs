using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace FTS
{
    internal sealed class Encryptor : IEncryptor
    {
        public byte[] Encrypt<T>(T data, byte[] salt, string password)
        {
            Rfc2898DeriveBytes key = new(password, salt);

            using Aes aesAlg = Aes.Create();
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

            BinaryFormatter formatter = new();
            using MemoryStream memoryStream = new();
            using (CryptoStream cryptoStream = new(memoryStream, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
            {
                formatter.Serialize(cryptoStream, data);
            }
            
            return memoryStream.ToArray();
        }
    }

    internal interface IEncryptor
    {
        public byte[] Encrypt<T>(T data, byte[] salt, string password);
    }
}