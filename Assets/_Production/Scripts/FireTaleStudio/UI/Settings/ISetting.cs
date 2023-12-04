using System;

namespace FTS.UI.Settings
{
    internal interface ISetting
    {
        public int Name { get; }
        public object Value { get; }
        public void Initialize(Action<ISetting> onValueChange, object sliderValue);
        public void SetValue(object value);
        public void ApplyData();
    }
}