using System;
using FTS.Tools.ExtensionMethods;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Profiles
{
    internal sealed class ProfileSlotUi : MenuButtonUi, IProfile
    {
        [SerializeField] private Button _profileButton;
        
        public int Name => Animator.StringToHash(name);
        private object Value { get; set; }
        
        public void Initialize(Action<IProfile> onProfileSelected, object profileValue)
        {
            Value = profileValue;
            _text.text = Value != null ? Value.ToString() : _profileButton.name.AddSpaceBetweenCapitalLetters();
            
            _profileButton.onClick.AddListener(() => onProfileSelected?.Invoke(this));
        }
            
        
        public void SetValue(object value)
        {
            Value = value;
            _text.text = Value != null ? Value.ToString() : _profileButton.name.AddSpaceBetweenCapitalLetters();
        }
    }

    internal interface IProfile
    {
        public int Name { get; }
        public void Initialize(Action<IProfile> onProfileSelected, object profileValue);
        public void SetValue(object value);
    }
}