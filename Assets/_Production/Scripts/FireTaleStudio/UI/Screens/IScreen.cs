using UnityEngine;

namespace FTS.UI.Screens
{
    internal interface IScreen
    {
        public GameObject ButtonObject { get; }
        public void Show(float? speed = null);
        public void Hide();
    }
}