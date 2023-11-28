using FTS.Tools.ScriptableEvents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Settings
{
    internal class SettingPrevNextBaseUi : SettingBaseUi
    {
        [SerializeField] protected TextMeshProUGUI _valueText;
        [SerializeField] protected Button _previousButton;
        [SerializeField] protected Button _nextButton;
        
        public override int Name => Animator.StringToHash(_valueText.name);
        public override object Value => _value; 
        
        public override void Initialize(EventInvoker<ISetting> onValueChange, object prevNextValue)
        {
            InitializeValue(onValueChange, prevNextValue);
            InitializeButtons(onValueChange);
        }

        protected virtual void InitializeButtons(EventInvoker<ISetting> onValueChange) { }
        protected virtual void InitializeValue(EventInvoker<ISetting> onValueChange, object prevNextValue) { }
        public override void ApplyData() { }
    }
}