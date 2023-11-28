using System;
using System.Globalization;
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
        public override object Value => (byte)_slider.value;

        public override void Initialize(EventInvoker<ISetting> onValueChange, object sliderValue)
        {
            _slider.value = Convert.ToByte(sliderValue);
            _valueText.text = sliderValue.ToString();

            _slider.onValueChanged.AddListener(value =>
            {
                _valueText.text = value.ToString(CultureInfo.InvariantCulture);
                onValueChange.Null()?.Raise(this);
            });
            
            onValueChange.Null()?.Raise(this);
        }

        public override void ApplyData() { }
    }
}