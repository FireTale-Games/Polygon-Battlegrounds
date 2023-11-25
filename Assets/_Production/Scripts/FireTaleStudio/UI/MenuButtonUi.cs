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
            GetComponent<Button>().onClick.AddListener(() => ButtonHandler?.Press(this));
        public void OnPointerEnter(PointerEventData eventData) =>
            ButtonHandler?.Enter(this);
        public void OnPointerExit(PointerEventData eventData) =>
            ButtonHandler?.Exit(this);

        public void SetTextColor(Color color) => 
            _text.color = color;
    }
}