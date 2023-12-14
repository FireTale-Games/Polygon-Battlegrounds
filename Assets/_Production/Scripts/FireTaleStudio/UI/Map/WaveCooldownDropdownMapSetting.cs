namespace FTS.UI.Map
{
    internal sealed class WaveCooldownDropdownMapSetting : MapDropdownSetting
    {
        public override float GetDropdownValue() =>
            _dropdown.value switch
            {
                0 => 10.0f, 
                1 => 15.0f, 
                2 => 20.0f,
                3 => 25.0f,
                4 => 30.0f,
                _ => 20.0f
            };

        public override void SetLabelValue(float value) =>
            _label.text = $"{value}s";
    }
}