using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FTS.UI.Settings;
using UnityEngine;

namespace FTS.Save
{
    public class SaveLoadData
    {
        private readonly string saveFilePath = Application.persistentDataPath + "/GameSettings.save";
        private Dictionary<int, object> gameSettings;

        public SaveLoadData() => 
            LoadSettingsData();

        private void LoadSettingsData()
        {
            if (File.Exists(saveFilePath))
            {
                BinaryFormatter formatter = new();
                using FileStream fileStream = new(saveFilePath, FileMode.Open);
                gameSettings = formatter.Deserialize(fileStream) as Dictionary<int, object>;
            }
            else
                gameSettings = new Dictionary<int, object>();
        }

        public bool SaveGameSetting(ISetting setting)
        {
            gameSettings[setting.Name] = setting.Value;
            return SaveDataToFile();
        }
        
        public object GetGameSetting(int settingName) => 
            gameSettings.TryGetValue(settingName, out object setting) ? setting : null;

        private bool SaveDataToFile()
        {
            try
            {
                BinaryFormatter formatter = new();
                using FileStream fileStream = new(saveFilePath, FileMode.Create);
                formatter.Serialize(fileStream, gameSettings);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}