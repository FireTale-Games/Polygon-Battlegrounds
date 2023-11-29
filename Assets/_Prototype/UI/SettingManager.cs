using System.Collections.Generic;
using FTS.Data;
using FTS.Tools.ExtensionMethods;
using FTS.Tools.ScriptableEvents;
using FTS.UI.Settings;
using UnityEngine;

namespace FTS.Managers
{
    internal sealed class SettingManager : MonoBehaviour
    {
        private EventObserver<ISetting> OnSettingData => _onSettingData ??= ExtensionMethods.LoadEventObject<ISetting>(nameof(OnSettingData));
        private EventObserver<ISetting> _onSettingData;
        
        private EventObserver<bool> OnSettingsApply => _onSettingsApply ??= ExtensionMethods.LoadEventObject<bool>(nameof(OnSettingsApply));
        private EventObserver<bool> _onSettingsApply;
        
        private Dictionary<int, object> _currentSettings;
        private readonly HashSet<ISetting> _previewDataSettings = new();
        
        private void SettingData(ISetting setting)
        {
            _previewDataSettings.Add(setting);
            setting.ApplyData();
        }
        
        private void SettingsApply(bool isApply)
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
        
        private void Awake()
        {
            _currentSettings = new DataLoader<Dictionary<int, object>, SettingManager>().LoadData() ?? new Dictionary<int, object>();
            OnSettingData.Null()?.AddObserver(SettingData);
            OnSettingsApply.Null()?.AddObserver(SettingsApply);
        }

        private void OnDestroy()
        {
            OnSettingData.Null()?.RemoveObserver(SettingData);
            OnSettingsApply.Null()?.RemoveObserver(SettingsApply);
        }

        public void SetInitialValues(IEnumerable<ISetting> settings)
        {
            IDataSaver<Dictionary<int, object>> saver = new DataSaver<Dictionary<int, object>, SettingManager>();
            EventInvoker<ISetting> OnSettingInvoker = ExtensionMethods.LoadEventObject<ISetting>(nameof(OnSettingData));
            foreach (ISetting setting in settings)
                setting.Initialize(OnSettingInvoker, GetSavedValue(setting));
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