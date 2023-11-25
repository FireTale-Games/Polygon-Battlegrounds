using System;
using FTS.Save;

namespace FTS.UI.Screens
{
    internal sealed class SettingsScreen : MenuScreenBase
    {
        private Action<string, object> OnSettingValueChange;
        
        protected override void Awake()
        {
            base.Awake();
            InitializeSettings();
        }

        private void OnDestroy() =>
            OnSettingValueChange -= SettingValueChange;
        
        private void SettingValueChange(string sliderName, object value) => 
            SaveManager.SaveGameSetting(new Save.Save(sliderName, value));

        private void InitializeSettings()
        {
            OnSettingValueChange += SettingValueChange;
            
            ISetting[] _settingsSliders = GetComponentsInChildren<ISetting>();
            foreach (ISetting _settingsSlider in _settingsSliders)
                _settingsSlider.Initialize(OnSettingValueChange, GetSavedSliderValue(_settingsSlider.Name, _settingsSlider.Value));
        }

        private object GetSavedSliderValue(string sliderName, object sliderValue)
        {
            object savedValue = SaveManager.GetGameSetting(sliderName);
            if (savedValue == null)
                SaveManager.SaveGameSetting(new Save.Save(sliderName, sliderValue));
            
            return savedValue ?? sliderValue;
        }
    }
}