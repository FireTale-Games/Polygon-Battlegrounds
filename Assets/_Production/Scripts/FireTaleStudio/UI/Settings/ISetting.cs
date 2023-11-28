using FTS.Tools.ScriptableEvents;

namespace FTS.UI.Settings
{
    public interface ISetting
    {
        public int Name { get; }
        public object Value { get; }
        public void Initialize(EventInvoker<ISetting> onValueChange, object sliderValue);
        public void ApplyData();
    }
}