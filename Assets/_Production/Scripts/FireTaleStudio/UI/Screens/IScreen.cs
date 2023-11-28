using System;
using UnityEngine;

namespace FTS.UI.Screens
{
    public interface IScreen
    {
        public GameObject ButtonObject { get; }
        public Action OnRequestToClose { get; set; }
        public Action<IScreen> OnRequestToOpen { get; set; }
        public void Show(float? speed = null);
        public void Hide(float? speed = null);
    }
}