using System;
using FTS.Tools.ExtensionMethods;
using FTS.Tools.ScriptableEvents;
using UnityEngine;

namespace FTS.UI.Settings
{
    internal sealed class DisplaySettingsUi : SettingPrevNextBaseUi
    {
        [SerializeField] private string[] fullScreenDisplay;
        public override object Value => _value ?? 0;
        
        protected override void InitializeButtons(EventInvoker<ISetting> onValueChange)
        {
            int value = Convert.ToInt32(Value);
            int enumLength = Enum.GetValues(typeof(FullScreenMode)).Length;

            _previousButton.onClick.AddListener(() => UpdateValue(false));
            _nextButton.onClick.AddListener(() => UpdateValue(true));
            return;
            
            void UpdateValue(bool isIncrement)
            {
                do value = isIncrement ? (value + 1) % enumLength : (value - 1 + enumLength) % enumLength;
                while (value == 2);

                _value = value;
                onValueChange.Null()?.Raise(this);
            }
        }

        protected override void InitializeValue(EventInvoker<ISetting> onValueChange, object prevNextValue)
        {
            _value = prevNextValue ?? 0;
            onValueChange.Null()?.Raise(this);
        }

        public override void ApplyData()
        {
            byte value = Convert.ToByte(Value);
            Screen.fullScreenMode = (FullScreenMode)Enum.GetValues(typeof(FullScreenMode)).GetValue(value);
            _valueText.text = fullScreenDisplay[value];
        }
    }
}