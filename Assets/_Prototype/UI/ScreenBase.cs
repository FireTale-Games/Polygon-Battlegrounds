using System;
using FTS.Tools.ExtentionMethods;
using UnityEngine;

namespace FTS.UI.Screens
{
    public abstract class ScreenBase : MonoBehaviour, IScreen
    {
        public Action OnRequestToClose { get; }
        public Action<IScreen> OnRequestToOpen { get; }
        
        public bool IsOpen { get; protected set; }

        protected CanvasGroup CanvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();
        private CanvasGroup _canvasGroup;

        protected Canvas Canvas => _canvas ??= GetComponent<Canvas>();
        private Canvas _canvas;

        protected virtual int SortOrderOnOpen => 4;

        protected virtual void Awake()
        {
            CanvasGroup.Null()?.HideCanvasGroup(0);
            if(Canvas != null)
                Canvas.sortingOrder = 1;
        }

        public virtual void Show()
        {
            IsOpen = true;
            CanvasGroup.Null()?.ShowCanvasGroup(0.1f);
            if (Canvas != null)
                Canvas.sortingOrder = SortOrderOnOpen;
        }

        public virtual void Hide()
        {
            IsOpen = false;
            CanvasGroup.Null()?.HideCanvasGroup(0.1f);
            if (Canvas != null)
                Canvas.sortingOrder = 1;
        }
    }
}