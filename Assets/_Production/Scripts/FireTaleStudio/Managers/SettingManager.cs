using System;
using System.Collections.Generic;
using FTS.Data;
using FTS.UI.Settings;

namespace FTS.Managers
{
    internal sealed class SettingManager : BaseManager
    {
        private Dictionary<int, object> _currentSettings;
        private readonly HashSet<ISetting> _previewDataSettings = new();

        public EventHandler<ISetting> OnSettingApplied;
        
        private void SettingData(ISetting setting)
        {
            _previewDataSettings.Add(setting);
            setting.ApplyData();
            OnSettingApplied?.Invoke(this, setting);
        }
        
        public void SettingsApply(bool isApply)
        {
            if (isApply)
            {
                foreach (ISetting setting in _previewDataSettings)
                    _currentSettings[setting.Name] = setting.Value;

                new DataSaver<Dictionary<int, object>, SettingManager>().SaveData(_currentSettings);
                _previewDataSettings.Clear();
                return;
            }

            foreach (ISetting setting in _previewDataSettings)
                setting.SetValue(_currentSettings[setting.Name]);
            
            _previewDataSettings.Clear();
        }
        
        private void Awake() => 
            _currentSettings = new DataLoader<Dictionary<int, object>, SettingManager>().LoadData() ?? new Dictionary<int, object>();
        
        public void SetInitialValues(IEnumerable<ISetting> settings)
        {
            IDataSaver<Dictionary<int, object>> saver = new DataSaver<Dictionary<int, object>, SettingManager>();
            Action<ISetting> OnSettingData = SettingData;
            foreach (ISetting setting in settings)
                setting.Initialize(OnSettingData, GetSavedValue(setting));
            return;

            object GetSavedValue(ISetting setting)
            {
                if (_currentSettings.TryGetValue(setting.Name, out object savedValue))
                    return savedValue ?? setting.Value;
                
                _currentSettings[setting.Name] = setting.Value;
                saver.SaveData(_currentSettings);
                return setting.Value;
            }
        }
    }
}