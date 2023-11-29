using System;
using System.IO;
using FTS.Cryptography;
using UnityEngine;

namespace FTS.Data
{
    internal sealed class DataLoader<T, TI> : IDataLoader<T>
    {
        private readonly string _saveFilePath = $"{Application.persistentDataPath}/{typeof(TI).Name}.save";
        private readonly IDecryptor _decryptor = new Decryptor();
        private readonly string _password = new PasswordGeneration().GetPassword(typeof(TI));

        public T LoadData()
        {
            if (!File.Exists(_saveFilePath)) 
                return default;
            
            // Read file data
            byte[] fileData = File.ReadAllBytes(_saveFilePath);
                
            // Get salt data
            byte[] salt = new byte[16];
            Array.Copy(fileData, 0, salt, 0, 16);
                
            // Get Encrypted data
            byte[] encryptedData = new byte[fileData.Length - 16];
            Array.Copy(fileData, 16, encryptedData, 0, encryptedData.Length);
                
            return _decryptor.Decrypt<T>(encryptedData, salt, _password);
        }
    }

    internal interface IDataLoader<out T>
    {
        public T LoadData();
    }
}