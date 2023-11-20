using FTS.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FTS.UI
{
    public class MenuButtonUi : MonoBehaviour, IMenuButtonUi, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeReference] private TextMeshProUGUI _text; 
        [field: SerializeField] public ScreenBase Screen { get; private set; }

        private IButtonHandler<IMenuButtonUi> ButtonHandler => _buttonHandler ??= GetComponentInParent<IButtonHandler<IMenuButtonUi>>();
        private IButtonHandler<IMenuButtonUi> _buttonHandler;

        public void OnPointerEnter(PointerEventData eventData) =>
            ButtonHandler?.OnEnter(this);

        public void OnPointerExit(PointerEventData eventData) =>
            ButtonHandler?.OnExit(this);

        public void OnPointerDown(PointerEventData eventData) =>
            ButtonHandler?.OnClick(this);

        public IScreen OnInteract(Color color)
        {
            SetTextColor(color);
            return Screen.GetComponent<IScreen>();
        }

        public void SetTextColor(Color color) => 
            _text.color = color;
    }
}