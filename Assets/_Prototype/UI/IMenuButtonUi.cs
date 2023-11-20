using FTS.UI.Screens;
using UnityEngine;

namespace FTS.UI
{
    public interface IMenuButtonUi
    {
        public IScreen OnInteract(Color color);
        public void SetTextColor(Color color);
    }
}