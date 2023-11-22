using System;
using UnityEngine;

namespace FTS.UI.Screens
{
    public class SettingsScreen : MenuScreenBase
    {
        private Action<string, int> OnSliderValueChanged;
        
        protected override void Awake()
        {
            base.Awake();
            InitializeSliders();
        }

        private void OnDestroy() =>
            DeinitializeActions();
        
        private void SliderValueChange(string sliderName, int value)
        {
            Debug.Log($"Name of the slider is: {sliderName} and value is: {value}");
        }
        
        private void InitializeSliders()
        {
            OnSliderValueChanged += SliderValueChange;
            
            ISettingsSlider[] _settingsSliders = GetComponentsInChildren<ISettingsSlider>();
            foreach (ISettingsSlider _settingsSlider in _settingsSliders)
                _settingsSlider.InitializeSlider(OnSliderValueChanged);
        }

        private void DeinitializeActions()
        {
            OnSliderValueChanged -= SliderValueChange;
        }
    }
}