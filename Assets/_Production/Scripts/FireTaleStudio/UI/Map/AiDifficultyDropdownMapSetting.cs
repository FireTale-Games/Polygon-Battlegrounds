namespace FTS.UI.Map
{
    internal sealed class AiDifficultyDropdownMapSetting : MapDropdownSetting
    {
        public override float GetDropdownValue() =>
            _dropdown.value switch
            {
                0 => 0.5f, 
                1 => 1.0f, 
                2 => 2.0f,
                _ => 1.0f
            };

        public override void SetLabelValue(float value) =>
            _label.text = value switch
            {
                0.5f => "Easy",
                1.0f => "Medium",
                2.0f => "Hard",
                _ => "Normal"
            };
    }
}
