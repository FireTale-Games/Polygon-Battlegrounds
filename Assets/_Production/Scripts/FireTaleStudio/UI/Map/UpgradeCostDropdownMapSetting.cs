namespace FTS.UI.Map
{
    internal sealed class UpgradeCostDropdownMapSetting : MapDropdownSetting
    {
        public override float GetDropdownValue() =>
            _dropdown.value switch
            {
                0 => 0.5f, 
                1 => 0.75f, 
                2 => 1.0f,
                3 => 1.25f,
                4 => 1.5f,
                5 => 1.75f,
                6 => 2.0f,
                _ => 1.0f
            };

        public override void SetLabelValue(float value) =>
            _label.text = $"{value * 100}%";
    }
}