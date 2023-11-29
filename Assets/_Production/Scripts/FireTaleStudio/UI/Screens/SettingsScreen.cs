using FTS.Managers;
using FTS.Save;
using FTS.Tools.ExtensionMethods;
using FTS.Tools.ScriptableEvents;
using FTS.UI.Settings;

namespace FTS.UI.Screens
{
    internal sealed class SettingsScreen : MenuScreenBase
    {
        private void Start() => 
            InitializeOptions();

        private void InitializeOptions()
        {
            EventInvoker<ISetting> OnSettingData = ExtensionMethods.LoadEventObject<ISetting>(nameof(OnSettingData));

            ISetting[] _settings = GetComponentsInChildren<ISetting>();
            foreach (ISetting setting in _settings)
                setting.Initialize(OnSettingData, GetSavedValue(setting));
        }

        private object GetSavedValue(ISetting setting)
        {
            SaveLoadData<SettingManager> saveLoadData = FindObjectOfType<SettingManager>().saveLoadData;
            
            object savedValue = saveLoadData.GetGameSetting(setting.Name);
            if (savedValue == null)
                saveLoadData.SaveGameSetting(setting.Name, setting.Value);
            
            return savedValue ?? setting.Value;
        }
    }
}