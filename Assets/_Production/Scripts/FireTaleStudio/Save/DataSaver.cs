using System;
using System.IO;
using FTS.Cryptography;
using UnityEngine;

namespace FTS.Data
{
    internal sealed class DataSaver<T> : IDataSaver<T>
    {
        private readonly string _saveFilePath = $"{Application.persistentDataPath}/{typeof(T).Name}.save";
        private readonly IEncryptor _encryptor = new Encryptor();
        private readonly ISaltGeneration _saltGenerator = new SaltGeneration();
        private readonly string _password = new PasswordGeneration().GetPassword(typeof(T).BaseType);

        public bool SaveData(T data)
        {
            try
            {
                byte[] salt = _saltGenerator.GetSaltArray(16); // Generate a new salt
                byte[] encryptedData = _encryptor.Encrypt(data, salt, _password);

                byte[] dataToSave = new byte[salt.Length + encryptedData.Length];
                Array.Copy(salt, 0, dataToSave, 0, salt.Length);
                Array.Copy(encryptedData, 0, dataToSave, salt.Length, encryptedData.Length);

                File.WriteAllBytes(_saveFilePath, dataToSave);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    internal interface IDataSaver<in T>
    {
        public bool SaveData(T data);
    }
}