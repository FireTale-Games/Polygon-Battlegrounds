using System;
using FTS.Tools.ExtensionMethods;
using FTS.Tools.ScriptableEvents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Settings
{
    internal class SettingSliderBaseUi : SettingBaseUi
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _valueText;

        public override int Name =>  Animator.StringToHash(_slider.name);
        public override object Value => _value ?? 100;

        public override void Initialize(EventInvoker<ISetting> onValueChange, object sliderValue)
        {
            _value = Convert.ToByte(sliderValue);
            _slider.onValueChanged.AddListener(value =>
            {
                _value = Convert.ToByte(value);
                onValueChange.Null()?.Raise(this);
            });
            
            onValueChange.Null()?.Raise(this);
        }

        public override void ApplyData()
        {
            byte value = Convert.ToByte(Value);
            _slider.value = value;
            _valueText.text = value.ToString();
        }
    }
}