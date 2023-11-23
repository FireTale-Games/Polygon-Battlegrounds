using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI
{
    public abstract class SettingsPrevNextUi : MonoBehaviour, ISetting
    {
        [SerializeField] protected TextMeshProUGUI _valueText;
        [SerializeField] protected Button _previousButton;
        [SerializeField] protected Button _nextButton;
        
        public string Name => _valueText.name;
        public virtual object Value => _valueText.text;
        
        public void Initialize(Action<string, object> onValueChange, object sliderValue)
        {
            _valueText.text = sliderValue.ToString();
            InitializeButtons(onValueChange);
        }

        protected abstract void InitializeButtons(Action<string, object> onValueChange);
    }
}