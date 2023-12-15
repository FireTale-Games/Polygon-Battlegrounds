using UnityEngine;

namespace FTS.UI.Map
{
    internal sealed class MonsterRarityCheckerMapSetting : MapCheckerSetting
    {
        public override bool GetCheckerValue() => _isEnabled;

        public override void SetValue(Sprite sprite)
        {
            _button.enabled = true;
            _buttonImage.sprite = sprite;
            _display.sprite = sprite;
        }
    }
}