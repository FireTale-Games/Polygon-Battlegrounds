using FTS.Tools.ScriptableEvents;
using UnityEngine;

namespace FTS.UI.Settings
{
    internal abstract class SettingBaseUi : MonoBehaviour, ISetting
    {
        public abstract int Name { get; }
        public abstract object Value { get; }
        protected object _value;
        public abstract void Initialize(EventInvoker<ISetting> onValueChange, object sliderValue);
        public void SetValue(object value)
        {
            _value = value;
            ApplyData();
        }

        public abstract void ApplyData();
    }
}