using FTS.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FTS.UI
{
    internal class MenuButtonUi : MonoBehaviour, IMenuButtonUi, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeReference] protected TextMeshProUGUI _text; 
        [field: SerializeField] protected ScreenBase Screen { get; private set; }
        public IScreen ButtonScreen => Screen;
        
        private IButtonHandler<IMenuButtonUi> ButtonHandler => _buttonHandler ??= GetComponentInParent<IButtonHandler<IMenuButtonUi>>();
        private IButtonHandler<IMenuButtonUi> _buttonHandler;

        public void SetBaseScreen(MenuScreenBase _menuScreenBase) =>
            Screen = _menuScreenBase;
        
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
        
        public void SetTextColor(Color textColor) => 
            _text.color = textColor;
    }
}