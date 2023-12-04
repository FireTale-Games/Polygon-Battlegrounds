using System;
using FTS.UI.Screens;
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
        private object Value { get; set; }
        
        public void Initialize(Action<IProfile> onProfileSelected, object profileValue)
        {
            Value = profileValue;
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => BindButton(onProfileSelected));
            _text.text = Value != null ? Value.ToString() : _button.name;
        }

        public void SetValue(object value)
        {
            Value = value;
            _text.text = value.ToString();
        }

        private void BindButton(Action<IProfile> onProfileSelected)
        {
            if (Value != null)
            {
                onProfileSelected?.Invoke(this);
                return;
            }
            
            GetComponentInParent<ISMScreen>().CreateNewProfile();
        }
    }

    internal interface IProfile
    {
        public int Name { get; }
        public void Initialize(Action<IProfile> onProfileSelected, object profileValue);
        public void SetValue(object value);
    }
}