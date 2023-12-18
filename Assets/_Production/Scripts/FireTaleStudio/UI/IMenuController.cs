using System;
using FTS.UI.Screens;

namespace FTS.UI
{
    internal interface IMenuController<T>
    {
        public Action<T> OnEnter { get; set; }
        public Action<T> OnExit { get; set; }
        public Action<T> OnPress { get; set; }
        public Action<IScreen> OnScreenChange { get; set; }
    }
}