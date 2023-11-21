using System;
using FTS.UI.Screens;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FTS.UI
{
    public class MainMenuUiController : MonoBehaviour, IButtonHandler<IMenuButtonUi>
    {
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _hoveredColor;
        [SerializeField] private Color _selectedColor;
        
        private IMenuButtonUi _currentButton;
        private IScreen _currentScreen;
        private EventSystem _eventSystem;
        
        private Action _closeRequestAction;
        private Action<IScreen> _openRequestAction;

        private void Awake() => 
            _eventSystem = EventSystem.current;

        public void OnEnter(IMenuButtonUi button)
        {
            _eventSystem.SetSelectedGameObject((button as MenuButtonUi)?.gameObject);
            button.SetTextColor(_currentButton == button ? _selectedColor : _hoveredColor);
        }

        public void OnExit(IMenuButtonUi button)
        {
            button.SetTextColor(_currentButton == button ? _selectedColor : _defaultColor);
        }

        public void OnClick(IMenuButtonUi button)
        {
            _currentButton?.SetTextColor(button == _currentButton ? _hoveredColor : _defaultColor);
            _currentButton = button == _currentButton ? null : button;
            
            if (_currentScreen != null)
                HideScreen(_currentScreen);
            
            if (_currentButton == null)
                return;
            
            ShowScreen(_currentButton.OnInteract(_selectedColor));
        }
        
        private void ShowScreen(IScreen screen)
        {
            _closeRequestAction = () => HideScreen(screen);
            _openRequestAction = s =>
            {
                HideScreen(screen);
                ShowScreen(s);
            };
            screen.OnRequestToClose += _closeRequestAction;
            screen.OnRequestToOpen += _openRequestAction;
            screen.Show();
            _currentScreen = screen;
        }
        
        private void HideScreen(IScreen screen)
        {
            screen.OnRequestToClose -= _closeRequestAction;
            screen.OnRequestToOpen -= _openRequestAction;
            _closeRequestAction = null;
            screen.Hide();
            _currentScreen = null;
        }
    }
}