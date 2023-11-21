using System;
using FTS.UI.Screens;
using UnityEngine;

namespace FTS.UI
{
    public abstract class UiMainController : MonoBehaviour, IButtonHandler<IMenuButtonUi>
    {
        [SerializeField] protected float _menuDisplaySpeed = 0.35f;
        [SerializeField] protected Color _defaultColor;
        [SerializeField] protected Color _hoveredColor;
        [SerializeField] protected Color _selectedColor;
        
        protected IScreen _currentScreen;

        private Action _closeRequestAction;
        private Action<IScreen> _openRequestAction;

        public abstract void OnEnter(IMenuButtonUi t);
        public abstract void OnExit(IMenuButtonUi t);
        public abstract void OnClick(IMenuButtonUi t);
        
        protected void ShowScreen(IScreen screen)
        {
            _closeRequestAction = () => HideScreen(screen);
            _openRequestAction = s =>
            {
                HideScreen(screen);
                ShowScreen(s);
            };
            screen.OnRequestToClose += _closeRequestAction;
            screen.OnRequestToOpen += _openRequestAction;
            screen.Show(_menuDisplaySpeed);
            _currentScreen = screen;
        }

        protected void HideScreen(IScreen screen)
        {
            screen.OnRequestToClose -= _closeRequestAction;
            screen.OnRequestToOpen -= _openRequestAction;
            _closeRequestAction = null;
            screen.Hide(_menuDisplaySpeed);
            _currentScreen = null;
        }
    }
}
