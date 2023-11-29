using FTS.Save;
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

        public SaveLoadData<SettingManager> saveLoadData;
        
        private void OnSetting(ISetting setting)
        {
            if (saveLoadData.SaveGameSetting(setting.Name, setting.Value))
                setting.ApplyData();
        }

        private void Awake()
        {
            saveLoadData = new SaveLoadData<SettingManager>();
            OnSettingData.Null()?.AddObserver(OnSetting);
        }

        private void OnDestroy() => 
            OnSettingData.Null()?.RemoveObserver(OnSetting);
    }
}