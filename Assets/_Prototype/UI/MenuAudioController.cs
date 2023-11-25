using System;
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
        [SerializeField] private EventInvoker<AudioClip> _invokeSound;
        [SerializeField] private EventInvoker<AudioClip> _invokeMusic;
        
        private void Start() => 
            _invokeMusic.Raise(_musicClip);

        private void OnEnter(IMenuButtonUi menuButton) => 
            _invokeSound.Raise(_hoverClip);

        private void OnExit(IMenuButtonUi menuButton) => 
            _invokeSound.Raise(_hoverClip);

        private void OnPress(IMenuButtonUi menuButton) => 
            _invokeSound.Raise(_pressClip);
        
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