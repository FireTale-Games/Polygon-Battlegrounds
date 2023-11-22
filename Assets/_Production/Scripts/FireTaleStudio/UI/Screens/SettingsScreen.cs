using System;
using FTS.Save;
using UnityEngine;

namespace FTS.UI.Screens
{
    public class SettingsScreen : MenuScreenBase
    {
        private Action<string, byte> OnSliderValueChanged;
        
        protected override void Awake()
        {
            base.Awake();
            InitializeSliders();
        }

        private void OnDestroy() =>
            DeinitializeActions();
        
        private void SliderValueChange(string sliderName, byte value)
        {
            Debug.Log($"Name of the slider is: {sliderName} and value is: {value}");
            SaveManager.SaveGameSetting(new Save.Save(sliderName, value));
        }
        
        private void InitializeSliders()
        {
            OnSliderValueChanged += SliderValueChange;
            
            ISettingsSlider[] _settingsSliders = GetComponentsInChildren<ISettingsSlider>();
            foreach (ISettingsSlider _settingsSlider in _settingsSliders)
                _settingsSlider.InitializeSlider(OnSliderValueChanged, GetSavedSliderValue(_settingsSlider.Name, _settingsSlider.Value));
        }

        private byte GetSavedSliderValue(string sliderName, byte sliderValue)
        {
            object savedValue = SaveManager.GetGameSetting(sliderName);
            if (savedValue == null)
                SaveManager.SaveGameSetting(new Save.Save(sliderName, sliderValue));
            
            return savedValue as byte? ?? sliderValue;
        }
        
        private void DeinitializeActions()
        {
            OnSliderValueChanged -= SliderValueChange;
        }
    }
}