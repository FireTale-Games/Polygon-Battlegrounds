using System;
using FTS.Tools.ExtensionMethods;
using FTS.Tools.ScriptableEvents;
using UnityEngine;

namespace FTS.UI.Settings
{
    internal sealed class QualitySettingUi : SettingPrevNextBaseUi
    {
        public override object Value => _value ?? QualitySettings.count - 1;
        
        protected override void InitializeButtons(EventInvoker<ISetting> onValueChange)
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
                onValueChange.Null()?.Raise(this);
            }
        }

        protected override void InitializeValue(EventInvoker<ISetting> onValueChange, object prevNextValue)
        {
            _value = prevNextValue ?? QualitySettings.count - 1;
            onValueChange.Null()?.Raise(this);
        }

        public override void ApplyData()
        {
            byte value = Convert.ToByte(Value);
            QualitySettings.SetQualityLevel(value);
            _valueText.text = QualitySettings.names[value];
        }
    }
}