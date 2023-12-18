using FTS.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FTS.UI
{
    internal class MenuButtonUi : MonoBehaviour, IMenuButtonUi, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        [Header("Components")]
        [SerializeReference] protected TextMeshProUGUI _text; 
        [field: SerializeField] protected ScreenBase Screen { get; private set; }
        public IScreen ButtonScreen => Screen;
        
        private IButtonHandler<IMenuButtonUi> ButtonHandler => _buttonHandler ??= GetComponentInParent<IButtonHandler<IMenuButtonUi>>();
        private IButtonHandler<IMenuButtonUi> _buttonHandler;
        
        private void Awake() => 
            GetComponent<Button>().onClick.AddListener(() => ButtonHandler?.Press(this));
        
        /// When the Mouse/Touch enters the Button fire an enter event to its handler.
        public void OnPointerEnter(PointerEventData eventData) =>
            ButtonHandler?.Enter(this);
        
        /// When the Mouse/Touch exits the Button fire an exit event to its handler.
        public void OnPointerExit(PointerEventData eventData) =>
            ButtonHandler?.Exit(this);

        /// When the Mouse/Touch selects the Button fire an enter event to its handler.
        public void OnSelect(BaseEventData eventData) => 
            ButtonHandler?.Enter(this);

        /// When the Mouse/Touch deselects the Button fire an exit event to its handler.
        public void OnDeselect(BaseEventData eventData) => 
            ButtonHandler?.Exit(this);
        
        public void SetTextColor(Color textColor) => 
            _text.color = textColor;
        
        /// Set the value of the screen that the buttons points to.
        public void SetBaseScreen(MenuScreenBase _menuScreenBase) =>
            Screen = _menuScreenBase;
    }
    
    /// Allows other components to interact with MenuButtonUi component.
    internal interface IMenuButtonUi
    {
        /// Screen to which the Button points to (for opening and closing the Screen).
        public IScreen ButtonScreen { get; }
        
        /// Set the color of the button.
        public void SetTextColor(Color color);
    }
}