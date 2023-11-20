using FTS.UI.Screens;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FTS.UI
{
    public class MenuButtonUi : MonoBehaviour, IMenuButtonUi, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [field: SerializeField] public ScreenBase Screen { get; private set; }

        private IButtonHandler<IMenuButtonUi> ButtonHandler => _buttonHandler ??= GetComponentInParent<IButtonHandler<IMenuButtonUi>>();
        private IButtonHandler<IMenuButtonUi> _buttonHandler;

        public void OnPointerEnter(PointerEventData eventData) =>
            ButtonHandler?.OnEnter(this);

        public void OnPointerExit(PointerEventData eventData) =>
            ButtonHandler?.OnExit(this);

        public void OnPointerDown(PointerEventData eventData) =>
            ButtonHandler?.OnClick(this);

        public IScreen OnInteract() => 
            Screen.GetComponent<IScreen>();
    }
}