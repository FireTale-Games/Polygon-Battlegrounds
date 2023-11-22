using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI
{
    public class SettingsUiSlider : MonoBehaviour, ISettingsSlider
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _valueText;

        public string Name => _slider.name;
        public byte Value => (byte)_slider.value;

        public void InitializeSlider(Action<string, byte> onValueChange, byte sliderValue)
        {
            _slider.value = sliderValue;
            _valueText.text = sliderValue.ToString();
            
            _slider.onValueChanged.AddListener(value =>
            {
                _valueText.text = value.ToString();
                onValueChange?.Invoke(_slider.name, (byte)value);  
            });
        }
    }

    public interface ISettingsSlider
    {
        public string Name { get; }
        public byte Value { get; }
        public void InitializeSlider(Action<string, byte> onValueChange, byte sliderValue);
    }
}