using UnityEngine.EventSystems;

namespace FTS.UI
{
    public class MainMenuUiController : UiMainController
    {
        private IMenuButtonUi _currentButton;
        private EventSystem _eventSystem;
        
        private void Awake() =>
            _eventSystem = EventSystem.current;
        
        public override void OnEnter(IMenuButtonUi button)
        {
            _eventSystem.SetSelectedGameObject((button as MenuButtonUi)?.gameObject);
            button.SetTextColor(_currentButton == button ? _selectedColor : _hoveredColor);
        }

        public override void OnExit(IMenuButtonUi button)
        {
            button.SetTextColor(_currentButton == button ? _selectedColor : _defaultColor);
        }

        public override void OnClick(IMenuButtonUi button)
        {
            _currentButton?.SetTextColor(button == _currentButton ? _hoveredColor : _defaultColor);
            _currentButton = button == _currentButton ? null : button;

            if (_currentScreen != null)
                HideScreen(_currentScreen);

            if (_currentButton == null)
                return;

            ShowScreen(_currentButton.OnInteract(_selectedColor));
        }
    }
}