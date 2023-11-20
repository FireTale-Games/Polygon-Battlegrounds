using System;

namespace FTS.UI.Screens
{
    public interface IScreen
    {
        public Action OnRequestToClose { get; set; }
        public Action<IScreen> OnRequestToOpen { get; set; }
        public bool IsOpen { get; }
        public void Show();
        public void Hide();
    }
}