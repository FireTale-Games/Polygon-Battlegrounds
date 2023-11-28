using FTS.Tools.ExtensionMethods;
using FTS.Tools.ScriptableEvents;
using UnityEngine;

namespace FTS.UI
{
    [DisallowMultipleComponent]
    internal sealed class MenuAudioController : MonoBehaviour
    {
        [SerializeField] private AudioClip _hoverClip;
        [SerializeField] private AudioClip _pressClip;
        [SerializeField] private AudioClip _musicClip;

        private EventInvoker<AudioClip> OnPlaySound => _onPlaySound ??= ExtensionMethods.LoadEventObject<AudioClip>(nameof(OnPlaySound));
        private EventInvoker<AudioClip> _onPlaySound;
        
        private EventInvoker<AudioClip> OnPlayMusic => _onPlayMusic ??= ExtensionMethods.LoadEventObject<AudioClip>(nameof(OnPlayMusic));
        private EventInvoker<AudioClip> _onPlayMusic;
        
        private void Start() => 
            OnPlayMusic.Null()?.Raise(_musicClip);

        private void OnEnter(IMenuButtonUi menuButton) => 
            OnPlaySound.Null()?.Raise(_hoverClip);
        
        private void OnPress(IMenuButtonUi menuButton) => 
            OnPlaySound.Null()?.Raise(_pressClip);
        
        private void OnEnable()
        {
            IMenuController<IMenuButtonUi> _controller = GetComponent<IMenuController<IMenuButtonUi>>();
            _controller.OnEnter += OnEnter;
            _controller.OnPress += OnPress;
        }

        private void OnDisable()
        {
            IMenuController<IMenuButtonUi> _controller = GetComponent<IMenuController<IMenuButtonUi>>();
            _controller.OnEnter -= OnEnter;
            _controller.OnPress -= OnPress;
        }
    }
}