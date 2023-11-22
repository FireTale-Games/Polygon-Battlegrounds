using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI
{
    public class SettingsUiSlider : MonoBehaviour, ISettingsSlider
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _text;
        
        public void InitializeSlider(Action<string, int> onValueChange)
        {
            _slider.onValueChanged.AddListener(value =>
            {
                int newValue = Mathf.RoundToInt(value * 100);
                _text.text = newValue.ToString();
                onValueChange?.Invoke(_slider.name, newValue);  
            });
        }
    }

    public interface ISettingsSlider
    {
        public void InitializeSlider(Action<string, int> onValueChange);
    }
}