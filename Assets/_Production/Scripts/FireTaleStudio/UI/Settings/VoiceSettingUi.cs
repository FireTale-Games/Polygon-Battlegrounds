namespace FTS.UI.Settings
{
    internal sealed class VoiceSettingUi : SettingSliderBaseUi
    {
        public override object Value => _value ?? 50;
    }
}