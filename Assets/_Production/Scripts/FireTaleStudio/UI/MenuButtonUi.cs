using FTS.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FTS.UI
{
    public class MenuButtonUi : MonoBehaviour, IMenuButtonUi, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeReference] private TextMeshProUGUI _text; 
        [field: SerializeField] public ScreenBase Screen { get; private set; }
        public IScreen ButtonScreen => Screen;
        
        private IButtonHandler<IMenuButtonUi> ButtonHandler => _buttonHandler ??= GetComponentInParent<IButtonHandler<IMenuButtonUi>>();
        private IButtonHandler<IMenuButtonUi> _buttonHandler;

        private void Awake() => 
            GetComponent<Button>().onClick.AddListener(() => ButtonHandler?.OnClick(this));
        public void OnPointerEnter(PointerEventData eventData) =>
            ButtonHandler?.OnEnter(this);
        public void OnPointerExit(PointerEventData eventData) =>
            ButtonHandler?.OnExit(this);

        public IScreen OnInteract(Color color)
        {
            SetTextColor(color);
            return Screen.GetComponent<IScreen>();
        }

        public void SetTextColor(Color color) => 
            _text.color = color;
    }
}