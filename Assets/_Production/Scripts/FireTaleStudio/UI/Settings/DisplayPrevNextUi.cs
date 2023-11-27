using System;
using UnityEngine;

namespace FTS.UI.Settings
{
    public class DisplayPrevNextUi : SettingsPrevNextUi
    {
        [SerializeField] private string[] fullScreenDisplay;
        public override object Value => _value ?? 0;
        
        protected override void InitializeButtons(Action<string, object> onValueChange)
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
                _valueText.text = fullScreenDisplay[value];
                onValueChange?.Invoke(_valueText.name, Value);
            }
        }

        protected override void InitializeValue(Action<string, object> onValueChange, object prevNextValue)
        {
            _value = prevNextValue ?? 0;
            if (prevNextValue == null)
                onValueChange?.Invoke(Name, Value);
            
            _valueText.text = fullScreenDisplay[Convert.ToByte(Value)];
        }
    }
}