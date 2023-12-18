using FTS.UI.Screens;
using UnityEngine;

namespace FTS.UI
{
    internal interface IMenuButtonUi
    {
        public IScreen ButtonScreen { get; }
        public void SetTextColor(Color color);
    }
}