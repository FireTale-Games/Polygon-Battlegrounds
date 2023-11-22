using DG.Tweening;
using FTS.Tools.ExtentionMethods;
using UnityEngine;

namespace FTS.UI.Screens
{
    public class MenuScreenBase : ScreenBase
    {
        [SerializeField] protected Vector2 _openedDimension;
        
        private Vector2 _originalPosition;
        private Vector2 _originalDimension;
        private RectTransform _rectTransform;
        private Sequence _mySequence;

        protected override void Awake()
        {
            base.Awake();
            
            _rectTransform = GetComponent<RectTransform>(); 
            _originalPosition = _rectTransform.position;
            _originalDimension = _rectTransform.rect.size;
        }

        public override void Show(float? speed = null)
        {
            float realSpeed = speed ?? _duration;
            base.Show(realSpeed);
            
            _mySequence?.Kill();
            _mySequence = DOTween.Sequence();
            _mySequence.Append(_rectTransform.DOLocalMove(Vector2.zero, realSpeed));
            _mySequence.Append(_rectTransform.DOSizeDelta(_openedDimension, realSpeed));
            _mySequence.Play().OnComplete(() => OnCompletePlay(realSpeed));
        }

        public override void Hide(float? speed = null)
        {
            float realSpeed = speed ?? _duration;
            base.Hide(realSpeed);
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).GetComponent<CanvasGroup>().HideCanvasGroup(0);
            
            _mySequence?.Kill();
            _mySequence = DOTween.Sequence();
            _mySequence.Append(_rectTransform.DOSizeDelta(_originalDimension, realSpeed));
            _mySequence.Append(_rectTransform.DOMove(_originalPosition, realSpeed));
            _mySequence.Play();
        }

        private void OnCompletePlay(float speed)
        {
            float partialDuration = speed / transform.childCount / 2.0f;
            for (int i = 0; i < transform.childCount; i++)
            {
                int index = i;
                DOVirtual.DelayedCall(i * partialDuration, () => transform.GetChild(index).GetComponent<CanvasGroup>().ShowCanvasGroup(partialDuration));
            }
        }
    }
}