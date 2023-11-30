using System.Collections.Generic;
using FTS.Tools.ExtensionMethods;
using FTS.Tools.ScriptableEvents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Profiles
{
    internal readonly struct ProfileToken
    {
        public readonly int _name;
        public readonly object _data;

        public ProfileToken(int name, object data)
        {
            _name = name;
            _data = data;
        }
    }
    
    internal sealed class ProfileSlotUi : MonoBehaviour, IProfile
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;
        
        public int Name => Animator.StringToHash(name);
        public object Value { get; private set; }

        public void Initialize(EventInvoker<IProfile> onValueChange, object profileValue)
        {
            Value = profileValue;
            _button.onClick.AddListener(() => onValueChange.Null()?.Raise(this));
            
            if (Value != null)
                onValueChange.Null()?.Raise(this);
        }

        public void SetValue(object value)
        {
            Value = value;
            _text.text = value.ToString();
        }
    }

    internal interface IProfile
    {
        public int Name { get; }
        public object Value { get; }
        public void Initialize(EventInvoker<IProfile> onValueChange, object profileValue);
        public void SetValue(object value);
    }
}