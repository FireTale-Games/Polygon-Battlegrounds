using System;

namespace FTS.UI
{
    public interface IMenuController<T>
    {
        public Action<T> OnEnter { get; set; }
        public Action<T> OnExit { get; set; }
        public Action<T> OnPress { get; set; }
    }
}