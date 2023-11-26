using System;
using FTS.Tools.ExtensionMethods;
using FTS.UI.Screens;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FTS.UI
{
    [RequireComponent(typeof(MainMenuUiController)), DisallowMultipleComponent]
    internal sealed class MenuScreenControllerUi : MonoBehaviour
    {
        private IScreen _currentScreen;
        private Action _closeRequestAction;
        private Action<IScreen> _openRequestAction;

        private GameObject _previousButtonObject;
        private EventSystem _eventSystem;

        private void Awake() => _eventSystem = EventSystem.current;

        private void OnEnter(IMenuButtonUi menuButton) => 
            _eventSystem.SetSelectedGameObject(ExtensionMethods.FirstRaycastHit());
        
        private void OnPress(IMenuButtonUi menuButton)
        {
            IScreen screen = menuButton.ButtonScreen;
            if (screen == null || screen == _currentScreen)
            {
                _eventSystem.SetSelectedGameObject(_previousButtonObject);
                _previousButtonObject = null;
                HideScreen(_currentScreen);
                return;
            }
            
            _previousButtonObject = _eventSystem.currentSelectedGameObject;
            _eventSystem.SetSelectedGameObject(screen.ButtonObject);
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
        
        private void OnEnable()
        {
            IMenuController<IMenuButtonUi> _controller = GetComponent<IMenuController<IMenuButtonUi>>();
            _controller.OnEnter += OnEnter;
            _controller.OnPress += OnPress;
        }

        private void OnDisable()
        {
            IMenuController<IMenuButtonUi> _controller = GetComponent<IMenuController<IMenuButtonUi>>();
            _controller.OnEnter -= OnEnter;
            _controller.OnPress -= OnPress;
        }
    }
}