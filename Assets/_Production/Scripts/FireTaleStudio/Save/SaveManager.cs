using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace FTS.Save
{
    public static class SaveManager
    {
        private static readonly string saveFilePath = Application.persistentDataPath + "/GameSettings.save";
        private static Dictionary<string, object> gameSettings;

        static SaveManager() => 
            LoadSettingsData();

        private static void LoadSettingsData()
        {
            if (File.Exists(saveFilePath))
            {
                BinaryFormatter formatter = new();
                using FileStream fileStream = new(saveFilePath, FileMode.Open);
                gameSettings = formatter.Deserialize(fileStream) as Dictionary<string, object>;
            }
            else
                gameSettings = new Dictionary<string, object>();
        }

        public static void SaveGameSetting(Save save)
        {
            gameSettings[save._name] = save._value;
            SaveDataToFile();
        }
        
        public static object GetGameSetting(string settingName) => 
            gameSettings.TryGetValue(settingName, out object setting) ? setting : null;

        private static void SaveDataToFile()
        {
            BinaryFormatter formatter = new();
            using FileStream fileStream = new(saveFilePath, FileMode.Create);
            formatter.Serialize(fileStream, gameSettings);
        }
    }
    
    [System.Serializable]
    public readonly struct Save
    {
        public readonly string _name;
        public readonly object _value;

        public Save(string name, object value)
        {
            _name = name;
            _value = value;
        }
    }
}