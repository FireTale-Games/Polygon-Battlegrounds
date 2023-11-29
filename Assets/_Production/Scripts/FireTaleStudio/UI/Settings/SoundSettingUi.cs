namespace FTS.UI.Settings
{
    internal sealed class SoundSettingUi : SettingSliderBaseUi
    {
        public override object Value => _value ?? 50;
    }
}