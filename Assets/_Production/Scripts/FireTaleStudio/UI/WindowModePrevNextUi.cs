using System;
using UnityEngine;

namespace FTS.UI
{
    public class WindowModePrevNextUi : SettingsPrevNextUi
    {
        public override object Value => string.IsNullOrEmpty(_valueText.text) ? FullScreenMode.ExclusiveFullScreen : _valueText.text;

        protected override void InitializeButtons(Action<string, object> onValueChange)
        {
            FullScreenMode screenType = (FullScreenMode)Enum.Parse(typeof(FullScreenMode), _valueText.text);
            int index = Array.IndexOf(Enum.GetValues(typeof(FullScreenMode)), screenType);

            _previousButton.onClick.AddListener(() => UpdateValue(false));
            _nextButton.onClick.AddListener(() => UpdateValue(true));
            return;

            void UpdateValue(bool isIncrement)
            {
                int enumLength = Enum.GetValues(typeof(FullScreenMode)).Length;

                do index = isIncrement ? (index + 1) % enumLength : (index - 1 + enumLength) % enumLength;
                while ((FullScreenMode)index == FullScreenMode.MaximizedWindow);

                _valueText.text = ((FullScreenMode)index).ToString();
                onValueChange?.Invoke(_valueText.name, _valueText.text);
            }
        }
    }
}