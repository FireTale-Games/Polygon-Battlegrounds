using System;
using UnityEngine;

namespace FTS.UI
{
    internal sealed class MainMenuUiController : MonoBehaviour, IButtonHandler<IMenuButtonUi>, IMenuController<IMenuButtonUi>
    {
        public Action<IMenuButtonUi> OnEnter { get; set; }
        public Action<IMenuButtonUi> OnExit { get; set; }
        public Action<IMenuButtonUi> OnPress { get; set; }

        public void Enter(IMenuButtonUi menuButton) => 
            OnEnter?.Invoke(menuButton);
        public void Exit(IMenuButtonUi menuButton) =>
            OnExit?.Invoke(menuButton);
        public void Press(IMenuButtonUi menuButton) =>
            OnPress?.Invoke(menuButton);
    }
}