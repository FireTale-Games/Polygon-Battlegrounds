using FTS.Tools.ExtensionMethods;
using FTS.Tools.ScriptableEvents;
using FTS.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FTS.UI.Profiles
{
    internal sealed class ProfileSlotUi : MonoBehaviour, IProfile, IMenuButtonUi, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private ScreenBase _screen;
        public IScreen ButtonScreen => _screen;
        
        public int Name => Animator.StringToHash(name);
        public object Value { get; private set; }

        private IButtonHandler<IMenuButtonUi> ButtonHandler => _buttonHandler ??= GetComponentInParent<IButtonHandler<IMenuButtonUi>>();
        private IButtonHandler<IMenuButtonUi> _buttonHandler;

        private void Awake() => 
            GetComponent<Button>().onClick.AddListener(() => ButtonHandler?.Press(this));
        
        public void OnPointerEnter(PointerEventData eventData) =>
            ButtonHandler?.Enter(this);
        
        public void OnPointerExit(PointerEventData eventData) =>
            ButtonHandler?.Exit(this);

        public void OnSelect(BaseEventData eventData) => 
            ButtonHandler?.Enter(this);

        public void OnDeselect(BaseEventData eventData) => 
            ButtonHandler?.Exit(this);
        
        public void Initialize(EventInvoker<IProfile> onValueChange, object profileValue)
        {
            Value = profileValue;
            _button.onClick.AddListener(() => onValueChange.Null()?.Raise(this));
            
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
        public void Initialize(EventInvoker<IProfile> onValueChange, object profileValue);
        public void SetValue(object value);
    }
}