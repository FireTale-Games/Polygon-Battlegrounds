using System;

namespace FTS.UI.Screens
{
    public interface IScreen
    {
        public Action OnRequestToClose { get; }
        public Action<IScreen> OnRequestToOpen { get; }
        public bool IsOpen { get; }
        public void Show();
        public void Hide();
    }
}