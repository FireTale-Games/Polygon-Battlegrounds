using System;

namespace FTS.UI.Settings
{
    public interface ISetting
    {
        public string Name { get; }
        public object Value { get; }
        public void Initialize(Action<string, object> onValueChange, object sliderValue);
    }
}