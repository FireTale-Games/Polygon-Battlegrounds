using DG.Tweening;
using UnityEngine;

namespace FTS.UI.Screens
{
    public class MenuScreenBase : ScreenBase
    {
        [SerializeField] protected Vector2 _openedDimension;
        
        private Vector2 _originalPosition;
        private Vector2 _originalDimension;
        private RectTransform _rectTransform;

        protected override void Awake()
        {
            base.Awake();
            
            _rectTransform = GetComponent<RectTransform>(); 
            _originalPosition = _rectTransform.position;
            _originalDimension = _rectTransform.rect.size;
        }

        public override void Show(float? speed = null)
        {
            base.Show(speed ?? _duration);
            _rectTransform.DOLocalMove(Vector2.zero, _duration).Play();
            _rectTransform.DOSizeDelta(_openedDimension, _duration).Play();
        }

        public override void Hide(float? speed = null)
        {
            base.Hide(speed ?? _duration);
            _rectTransform.DOMove(_originalPosition, _duration).Play();
            _rectTransform.DOSizeDelta(_originalDimension, _duration).Play();
        }
    }
}