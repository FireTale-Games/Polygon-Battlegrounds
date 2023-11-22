using FTS.UI.Screens;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FTS.UI
{
    public class MainMenuUiController : UiMainController
    {
        private IMenuButtonUi _currentButton;
        private IMenuButtonUi _previousButton;
        private EventSystem _eventSystem;
        
        private void Awake() =>
            _eventSystem = EventSystem.current;
        
        public override void OnEnter(IMenuButtonUi button)
        {
            _eventSystem.SetSelectedGameObject((button as MenuButtonUi)?.gameObject);
            button.SetTextColor(_currentButton == button ? _selectedColor : _hoveredColor);
        }

        public override void OnExit(IMenuButtonUi button) => 
            button.SetTextColor(_currentButton == button ? _selectedColor : _defaultColor);

        public override void OnClick(IMenuButtonUi button)
        {
            _currentButton?.SetTextColor(button == _currentButton ? _hoveredColor : _defaultColor);
            _currentButton = button == _currentButton ? null : button.ButtonScreen == _currentScreen ? null : button;

            if (_currentScreen != null)
            {
                _eventSystem.SetSelectedGameObject((_previousButton as MenuButtonUi)?.gameObject);
                _previousButton = null;
                HideScreen(_currentScreen);
            }

            if (_currentButton == null)
                return;

            IScreen screen = _currentButton.OnInteract(_selectedColor);
            _previousButton = _currentButton;
            _eventSystem.SetSelectedGameObject((screen as MenuScreenBase)?.GetComponentInChildren<MenuButtonUi>()?.gameObject);
            ShowScreen(screen);
        }
    }
}