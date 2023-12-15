using System;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Map
{
    internal abstract class MapCheckerSetting : MapBaseSetting
    {
        [Header("Data")]
        [SerializeField] protected Sprite _defaultSprite;
        [SerializeField] protected bool _isEnabled = true;
        
        [Header("Components")]
        [SerializeField] protected Button _button;
        [SerializeField] protected Image _buttonImage;
        [SerializeField] protected Image _display;
        
        public override void Initialize(Action onChangeData) => 
            _button.onClick.AddListener(() =>
            {
                _isEnabled = !_isEnabled;
                _button.enabled = false;
                onChangeData?.Invoke();
            });

        public override void SetDefaultValues(bool isHost)
        {
            _button.enabled = true;
            _button.gameObject.SetActive(isHost);
            _display.gameObject.SetActive(!isHost);
            if (isHost) _buttonImage.sprite = _defaultSprite;
            else _display.sprite = _defaultSprite;
        }

        public abstract bool GetCheckerValue();

        public virtual void SetValue(Sprite sprite) { _button.enabled = true; }

        private void OnDestroy() => _button.onClick.RemoveAllListeners();
    }  
}