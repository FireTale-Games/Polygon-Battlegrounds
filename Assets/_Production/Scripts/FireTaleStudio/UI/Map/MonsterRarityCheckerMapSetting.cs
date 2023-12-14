using UnityEngine;

namespace FTS.UI.Map
{
    internal sealed class MonsterRarityCheckerMapSetting : MapCheckerSetting
    {
        public override bool GetCheckerValue() => _isEnabled;

        public override void SetSpriteValue(Sprite sprite)
        {
            _buttonImage.sprite = sprite;
            _display.sprite = sprite;
        }
    }
}