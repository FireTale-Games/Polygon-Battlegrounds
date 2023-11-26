using System;
using FTS.Tools.ExtensionMethods;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    public abstract class ScreenBase : MonoBehaviour, IScreen
    {
        protected const float _duration = 0.1f;

        public GameObject ButtonObject => GetComponentInChildren<Button>().gameObject;
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

        public virtual void Show(float? speed = null)
        {
            CanvasGroup.Null()?.ShowCanvasGroup(speed ?? _duration);
            if (Canvas != null)
                Canvas.sortingOrder = SortOrderOnOpen;
        }

        public virtual void Hide(float? speed = null)
        {
            CanvasGroup.Null()?.HideCanvasGroup(speed ?? _duration);
            if (Canvas != null)
                Canvas.sortingOrder = 1;
        }
    }
}