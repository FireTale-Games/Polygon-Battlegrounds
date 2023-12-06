using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace FTS.Cryptography
{
    internal sealed class Decryptor : IDecryptor
    {
        public T Decrypt<T>(byte[] encryptedData, byte[] salt, string password)
        {
            Rfc2898DeriveBytes key = new(password, salt);

            using Aes aesAlg = Aes.Create();
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

            BinaryFormatter formatter = new();
            using MemoryStream memoryStream = new(encryptedData);
            using CryptoStream cryptoStream = new(memoryStream, aesAlg.CreateDecryptor(), CryptoStreamMode.Read);
            return (T)formatter.Deserialize(cryptoStream);
        }
    }

    internal interface IDecryptor
    {
        public T Decrypt<T>(byte[] encryptedData, byte[] salt, string password);
    }
}