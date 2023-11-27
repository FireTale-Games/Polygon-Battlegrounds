using System;
using UnityEngine;

namespace FTS.UI.Settings
{
    public class ResolutionPrevNextUi : SettingsPrevNextUi
    {
        public override object Value => _value ?? Screen.resolutions.Length - 1;
        
        protected override void InitializeButtons(Action<string, object> onValueChange)
        {
            Resolution[] resolutions = Screen.resolutions;
            int value = Convert.ToInt32(Value);
            
            _previousButton.onClick.AddListener(() => UpdateResolutionIndex(false));
            _nextButton.onClick.AddListener(() => UpdateResolutionIndex(true));
            return;
            
            void UpdateResolutionIndex(bool isIncrement)
            {
                value = isIncrement ? (value + 1) % resolutions.Length :
                    value == 0 ? resolutions.Length - 1 : value - 1;

                _value = value;
                _valueText.text = resolutions[value].ToString();
                onValueChange?.Invoke(_valueText.name, Value);
            }
        }

        protected override void InitializeValue(Action<string, object> onValueChange, object prevNextValue)
        {
            _value = prevNextValue ?? Screen.resolutions.Length - 1;
            if (prevNextValue == null)
                onValueChange?.Invoke(Name, Value);
            
            _valueText.text = Screen.resolutions[Convert.ToByte(Value)].ToString();
        }
    }
}
