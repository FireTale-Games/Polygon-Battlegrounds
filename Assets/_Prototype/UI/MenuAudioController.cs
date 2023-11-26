using FTS.Tools.ExtensionMethods;
using FTS.Tools.ScriptableEvents;
using UnityEngine;

namespace FTS.UI
{
    [RequireComponent(typeof(MainMenuUiController)), DisallowMultipleComponent]
    internal sealed class MenuAudioController : MonoBehaviour
    {
        [SerializeField] private AudioClip _hoverClip;
        [SerializeField] private AudioClip _pressClip;
        [SerializeField] private AudioClip _musicClip;

        private EventInvoker<AudioClip> OnPlaySound => _onPlaySound ??= ExtensionMethods.LoadEventInvoker<AudioClip>(nameof(OnPlaySound));
        private EventInvoker<AudioClip> _onPlaySound;
        
        private EventInvoker<AudioClip> OnPlayMusic => _onPlayMusic ??= ExtensionMethods.LoadEventInvoker<AudioClip>(nameof(OnPlayMusic));
        private EventInvoker<AudioClip> _onPlayMusic;
        
        private void Start() => 
            OnPlayMusic.Raise(_musicClip);

        private void OnEnter(IMenuButtonUi menuButton) => 
            OnPlaySound.Raise(_hoverClip);

        private void OnExit(IMenuButtonUi menuButton) => 
            OnPlaySound.Raise(_hoverClip);

        private void OnPress(IMenuButtonUi menuButton) => 
            OnPlaySound.Raise(_pressClip);
        
        private void OnEnable()
        {
            IMenuController<IMenuButtonUi> _controller = GetComponent<IMenuController<IMenuButtonUi>>();
            _controller.OnEnter += OnEnter;
            _controller.OnExit += OnExit;
            _controller.OnPress += OnPress;
        }

        private void OnDisable()
        {
            IMenuController<IMenuButtonUi> _controller = GetComponent<IMenuController<IMenuButtonUi>>();
            _controller.OnEnter -= OnEnter;
            _controller.OnExit -= OnExit;
            _controller.OnPress -= OnPress;
        }
    }
}