namespace FTS.UI.Settings
{
    internal sealed class MusicSettingUi : SettingSliderBaseUi
    {
        public override object Value => _value ?? 50;
    }
}