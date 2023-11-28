using FTS.UI.Screens;
using UnityEngine;

namespace FTS.UI
{
    public interface IMenuButtonUi
    {
        public IScreen ButtonScreen { get; }
        public void SetTextColor(Color color);
    }
}