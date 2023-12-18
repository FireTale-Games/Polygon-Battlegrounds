using System;
using FTS.UI.Screens;

namespace FTS.UI
{
    /// <summary>
    /// Attach to main controller for the UI component.
    /// </summary>
    /// <typeparam name="T">Fire T type event to all subscribers</typeparam>
    internal interface IMenuController<T>
    {
        public Action<T> OnEnter { get; set; }
        public Action<T> OnExit { get; set; }
        public Action<T> OnPress { get; set; }
        
        /// When "Screen" changes the event should be fired.
        public Action<IScreen> OnScreenChange { get; set; }
    }
}