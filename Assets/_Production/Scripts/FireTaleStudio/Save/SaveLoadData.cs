using System;
using System.Collections.Generic;
using System.IO;
using FTS.Cryptography;
using UnityEngine;

namespace FTS.Save
{
    public class SaveLoadData<T>
    {
        private readonly string _saveFilePath = $"{Application.persistentDataPath}/{typeof(T).Name}.save";
        private Dictionary<int, object> _gameSettings;
        private readonly string _password = new PasswordGeneration().GetPassword(typeof(T).BaseType);
        private readonly byte[] _salt;

        private readonly IEncryptor _encryptor = new Encryptor();
        private readonly IDecryptor _decryptor = new Decryptor();
        private readonly ISaltGeneration _saltGeneration = new SaltGeneration();
        
        public SaveLoadData() => LoadSettingsData();

        private void LoadSettingsData()
        {
            if (File.Exists(_saveFilePath))
            {
                byte[] fileData = File.ReadAllBytes(_saveFilePath);
                byte[] salt = new byte[16];
                Array.Copy(fileData, 0, salt, 0, 16);
                
                byte[] encryptedData = new byte[fileData.Length - 16];
                Array.Copy(fileData, 16, encryptedData, 0, encryptedData.Length);

                _gameSettings = _decryptor.Decrypt<Dictionary<int, object>>(encryptedData, salt, _password);
            }
            else
                _gameSettings = new Dictionary<int, object>();
        }

        public bool SaveGameSetting(int name, object value)
        {
            _gameSettings[name] = value;
            return SaveDataToFile();
        }

        public object GetGameSetting(int settingName) => 
            _gameSettings.TryGetValue(settingName, out object setting) ? setting : null;

        private bool SaveDataToFile()
        {
            try
            {
                byte[] salt = _saltGeneration.GetSaltArray(16); // Generate a new salt
                byte[] encryptedData = _encryptor.Encrypt(_gameSettings, salt, _password);

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
}