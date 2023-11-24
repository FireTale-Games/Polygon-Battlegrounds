using System;
using UnityEngine;

namespace FTS.UI.Settings
{
    public class QualityPrevNextUi : SettingsPrevNextUi
    {
        public override object Value => _value ?? QualitySettings.count - 1;

        protected override void InitializeButtons(Action<string, object> onValueChange)
        {
            int value = Convert.ToInt32(Value);
            _previousButton.onClick.AddListener(() => UpdateResolutionIndex(false));
            _nextButton.onClick.AddListener(() => UpdateResolutionIndex(true));
            return;

            void UpdateResolutionIndex(bool isIncrement)
            {
                value = isIncrement ? (value + 1) % QualitySettings.names.Length :
                    value == 0 ? value = QualitySettings.names.Length - 1 : value - 1;

                _value = value;
                _valueText.text = QualitySettings.names[value];
                onValueChange?.Invoke(_valueText.name, Value);
            }
        }

        protected override void InitializeValue(Action<string, object> onValueChange, object prevNextValue)
        {
            _value = prevNextValue ?? QualitySettings.count - 1;
            if (prevNextValue == null)
                onValueChange?.Invoke(Name, Value);

            _valueText.text = QualitySettings.names[Convert.ToByte(Value)];
        }
    }
}