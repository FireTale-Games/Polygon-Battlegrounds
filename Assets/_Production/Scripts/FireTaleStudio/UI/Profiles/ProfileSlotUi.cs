using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Profiles
{
    internal sealed class ProfileSlotUi : MonoBehaviour, IProfile
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;
        
        public int Name => Animator.StringToHash(name);
        public object Value { get; private set; }
        
        public void Initialize(Action<IProfile> onValueChange, object profileValue)
        {
            Value = profileValue;
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => onValueChange?.Invoke(this));
            
            if (Value != null)
                _text.text = Value.ToString();
        }

        public void SetValue(object value)
        {
            Value = value;
            _text.text = value.ToString();
        }
        public void SetTextColor(Color color) { }
    }

    internal interface IProfile
    {
        public int Name { get; }
        public void Initialize(Action<IProfile> onValueChange, object profileValue);
        public void SetValue(object value);
    }
}