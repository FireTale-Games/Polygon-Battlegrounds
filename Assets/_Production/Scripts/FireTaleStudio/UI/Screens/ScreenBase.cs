using FTS.Tools.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal abstract class ScreenBase : MonoBehaviour, IScreen
    {
        protected const float _duration = 0.1f;

        public GameObject ButtonObject => GetComponentInChildren<Button>().gameObject;

        protected CanvasGroup CanvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();
        private CanvasGroup _canvasGroup;

        protected Canvas Canvas => _canvas ??= GetComponent<Canvas>();
        private Canvas _canvas;

        protected int SortOrderOnOpen => 4;

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

        public virtual void Hide()
        {
            CanvasGroup.Null()?.HideCanvasGroup(_duration);
            if (Canvas != null)
                Canvas.sortingOrder = 1;
        }
    }
}