using System;
using FTS.UI.Screens;
using UnityEngine;

namespace FTS.UI
{
    [DisallowMultipleComponent]
    internal sealed class MenuScreenControllerUi : MonoBehaviour
    {
        [SerializeField] private float _displaySpeed = 0.3f;
        
        private IScreen _currentScreen;
        private Action _closeRequestAction;
        private Action<IScreen> _openRequestAction;
        
        private void OnPress(IMenuButtonUi menuButton)
        {
            IScreen screen = menuButton.ButtonScreen;
            if (screen == null || screen == _currentScreen)
            {
                HideScreen(screen);
                GetComponent<MenuUiController>()?.ScreenChange();
                return;
            }
            
            if (_currentScreen != null)
                HideScreen(_currentScreen);
            ShowScreen(screen);
        }
        
        private void ShowScreen(IScreen screen)
        {
            screen.Show(_displaySpeed);
            _currentScreen = screen;
            GetComponent<MenuUiController>()?.ScreenChange(_currentScreen);
        }
        
        private void HideScreen(IScreen screen)
        {
            screen.Hide();
            _currentScreen = null;
        }
        
        private void OnEnable() => 
            GetComponent<IMenuController<IMenuButtonUi>>().OnPress += OnPress;

        private void OnDisable() => 
            GetComponent<IMenuController<IMenuButtonUi>>().OnPress -= OnPress;
    }
}