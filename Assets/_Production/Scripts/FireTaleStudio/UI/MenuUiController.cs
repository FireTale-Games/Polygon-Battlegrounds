using System;
using FTS.UI.Screens;
using UnityEngine;

namespace FTS.UI
{
    [RequireComponent(typeof(MenuScreenControllerUi), typeof(MenuAudioController), typeof(MenuButtonController)), DisallowMultipleComponent]
    internal sealed class MenuUiController : MonoBehaviour, IButtonHandler<IMenuButtonUi>, IMenuController<IMenuButtonUi>
    {
        public Action<IMenuButtonUi> OnEnter { get; set; }
        public Action<IMenuButtonUi> OnExit { get; set; }
        public Action<IMenuButtonUi> OnPress { get; set; }
        public Action<IScreen> OnScreenChange { get; set; }

        public void Enter(IMenuButtonUi menuButton) => 
            OnEnter?.Invoke(menuButton);
        public void Exit(IMenuButtonUi menuButton) =>
            OnExit?.Invoke(menuButton);
        public void Press(IMenuButtonUi menuButton) =>
            OnPress?.Invoke(menuButton);
        
        /// Screen has been changed, fire an event.
        /// null => there is no parent screen.
        /// !null => there is a parent screen.
        public void ScreenChange(IScreen screen = null) =>
            OnScreenChange?.Invoke(screen);
    }
}