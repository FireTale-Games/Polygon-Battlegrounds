using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Settings
{
    public class SettingUi : MonoBehaviour, ISetting
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _valueText;

        public string Name => _slider.name;
        public object Value => (byte)_slider.value;

        public void Initialize(Action<string, object> onValueChange, object sliderValue)
        {
            _slider.value = Convert.ToByte(sliderValue);
            _valueText.text = sliderValue.ToString();
            
            _slider.onValueChanged.AddListener(value =>
            {
                _valueText.text = value.ToString(CultureInfo.InvariantCulture);
                onValueChange?.Invoke(_slider.name, (byte)value);  
            });
        }
    }
}