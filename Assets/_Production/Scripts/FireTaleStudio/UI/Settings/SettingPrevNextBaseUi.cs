using System;
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
        
        public override void Initialize(Action<ISetting> onValueChange, object prevNextValue)
        {
            InitializeValue(onValueChange, prevNextValue);
            InitializeButtons(onValueChange);
        }

        protected virtual void InitializeButtons(Action<ISetting> onValueChange) { }
        protected virtual void InitializeValue(Action<ISetting> onValueChange, object prevNextValue) { }
        public override void ApplyData() { }
    }
}