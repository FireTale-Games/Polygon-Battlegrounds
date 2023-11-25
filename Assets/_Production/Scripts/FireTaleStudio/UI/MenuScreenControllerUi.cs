using System;
using FTS.UI.Screens;
using UnityEngine;

namespace FTS.UI
{
    [RequireComponent(typeof(MainMenuUiController)), DisallowMultipleComponent]
    internal sealed class MenuScreenControllerUi : MonoBehaviour
    {
        private IScreen _currentScreen;
        private Action _closeRequestAction;
        private Action<IScreen> _openRequestAction;
        
        private void OnPress(IMenuButtonUi menuButton)
        {
            IScreen screen = menuButton.ButtonScreen;
            if (screen == null)
                return;
            
            ShowScreen(screen);
        }
        
        private void ShowScreen(IScreen screen)
        {
            _closeRequestAction = () => HideScreen(_currentScreen);
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
        
        private void OnEnable() => 
            GetComponent<IMenuController<IMenuButtonUi>>().OnPress += OnPress;

        private void OnDisable() => 
            GetComponent<IMenuController<IMenuButtonUi>>().OnPress -= OnPress;
    }
}