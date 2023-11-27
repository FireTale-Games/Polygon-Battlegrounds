using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Settings
{
    public abstract class SettingsPrevNextUi : MonoBehaviour, ISetting
    {
        [SerializeField] protected TextMeshProUGUI _valueText;
        [SerializeField] protected Button _previousButton;
        [SerializeField] protected Button _nextButton;
        
        public string Name => _valueText.name;
        public virtual object Value => _value; 
        protected object _value;
        
        public void Initialize(Action<string, object> onValueChange, object prevNextValue)
        {
            InitializeValue(onValueChange, prevNextValue);
            InitializeButtons(onValueChange);
        }

        protected abstract void InitializeButtons(Action<string, object> onValueChange);
        protected abstract void InitializeValue(Action<string, object> onValueChange, object prevNextValue);
    }
}