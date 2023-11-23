using System;
using System.Linq;
using UnityEngine;

namespace FTS.UI
{
    public class ResolutionPrevNextUi : SettingsPrevNextUi
    {
        public override object Value => string.IsNullOrEmpty(_valueText.text) ? Screen.resolutions.Last().ToString() : _valueText.text;

        protected override void InitializeButtons(Action<string, object> onValueChange)
        {
            Resolution[] resolutions = Screen.resolutions;
            var resolutionWithIndex = resolutions.Select((res, index) => new { Resolution = res, Index = index }).FirstOrDefault(x => x.Resolution.ToString() == Value.ToString());
            int currentResolutionIndex = resolutionWithIndex?.Index ?? -1;
            
            _previousButton.onClick.AddListener(() => UpdateResolutionIndex(false));
            _nextButton.onClick.AddListener(() => UpdateResolutionIndex(true));
            
            return;

            void UpdateResolutionIndex(bool isIncrement)
            {
                if (isIncrement)
                    currentResolutionIndex = (currentResolutionIndex + 1) % resolutions.Length;
                else
                if (currentResolutionIndex == 0)
                    currentResolutionIndex = resolutions.Length - 1;
                else
                    currentResolutionIndex -= 1;

                _valueText.text = resolutions[currentResolutionIndex].ToString();
                onValueChange?.Invoke(_valueText.name, resolutions[currentResolutionIndex].ToString());
            }
        }
    }
}
