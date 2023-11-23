using System;

namespace FTS.UI
{
    public interface ISetting
    {
        public string Name { get; }
        public byte Value { get; }
        public void Initialize(Action<string, object> onValueChange, object sliderValue);
    }
}