using System;
using FTS.UI.Screens;
using UnityEngine;

namespace FTS.UI
{
    public class MainMenuUiController : MonoBehaviour, IButtonHandler<IMenuButtonUi>
    {
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _hoveredColor;
        [SerializeField] private Color _selectedColor;
        
        private IMenuButtonUi _currentButton;
        private IScreen _currentScreen;
        
        private Action _closeRequestAction;
        private Action<IScreen> _openRequestAction;

        public void OnEnter(IMenuButtonUi button) =>
            button.SetTextColor(_hoveredColor);

        public void OnExit(IMenuButtonUi button) => 
            button.SetTextColor(_currentButton == button ? _selectedColor : _defaultColor);

        public void OnClick(IMenuButtonUi button)
        {
            _currentButton?.SetTextColor(button == _currentButton ? _hoveredColor : _defaultColor);
            _currentButton = button == _currentButton ? null : button;
            
            if (_currentScreen != null)
                HideScreen(_currentScreen);
            
            if (_currentButton == null)
                return;
            
            ShowScreen(_currentButton.OnInteract(_hoveredColor));
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