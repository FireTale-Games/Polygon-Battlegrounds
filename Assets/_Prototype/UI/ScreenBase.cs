using System;
using FTS.Tools.ExtentionMethods;
using UnityEngine;

namespace FTS.UI.Screens
{
    public abstract class ScreenBase : MonoBehaviour, IScreen
    {
        public Action OnRequestToClose { get; set; }
        public Action<IScreen> OnRequestToOpen { get; set; }

        private CanvasGroup CanvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();
        private CanvasGroup _canvasGroup;

        private Canvas Canvas => _canvas ??= GetComponent<Canvas>();
        private Canvas _canvas;

        private int SortOrderOnOpen => 4;

        protected virtual void Awake()
        {
            CanvasGroup.Null()?.HideCanvasGroup(0);
            if(Canvas != null)
                Canvas.sortingOrder = 1;
        }

        public virtual void Show()
        {
            CanvasGroup.Null()?.ShowCanvasGroup(0.1f);
            if (Canvas != null)
                Canvas.sortingOrder = SortOrderOnOpen;
        }

        public virtual void Hide()
        {
            CanvasGroup.Null()?.HideCanvasGroup(0.1f);
            if (Canvas != null)
                Canvas.sortingOrder = 1;
        }
    }
}