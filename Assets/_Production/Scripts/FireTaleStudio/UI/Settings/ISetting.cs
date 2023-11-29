using FTS.Tools.ScriptableEvents;

namespace FTS.UI.Settings
{
    internal interface ISetting
    {
        public int Name { get; }
        public object Value { get; }
        public void Initialize(EventInvoker<ISetting> onValueChange, object sliderValue);
        public void SetValue(object value);
        public void ApplyData();
    }
}