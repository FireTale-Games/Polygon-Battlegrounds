using System;
using System.IO;
using FTS.Cryptography;
using UnityEngine;

namespace FTS.Data
{
    internal sealed class DataSaver<T, TI> : IDataSaver<T>
    {
        private readonly string _saveFilePath = $"{Application.persistentDataPath}/{typeof(TI).Name}";
        private readonly IEncryptor _encryptor = new Encryptor();
        private readonly ISaltGeneration _saltGenerator = new SaltGeneration();
        private readonly string _password = new PasswordGeneration().GetPassword(typeof(TI));

        internal DataSaver(string pathAddition = "") => 
            _saveFilePath += $"{pathAddition}.save";

        public void SaveData(T data)
        {
            try
            {
                byte[] salt = _saltGenerator.GetSaltArray(16); // Generate a new salt
                byte[] encryptedData = _encryptor.Encrypt(data, salt, _password);

                byte[] dataToSave = new byte[salt.Length + encryptedData.Length];
                Array.Copy(salt, 0, dataToSave, 0, salt.Length);
                Array.Copy(encryptedData, 0, dataToSave, salt.Length, encryptedData.Length);

                File.WriteAllBytes(_saveFilePath, dataToSave);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }

    internal interface IDataSaver<in T>
    {
        public void SaveData(T data);
    }
}