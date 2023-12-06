using System;
using FTS.Managers;
using UnityEngine;

namespace FTS.UI
{
    [DisallowMultipleComponent]
    internal sealed class MenuAudioController : MonoBehaviour
    {
        [SerializeField] private AudioClip _hoverClip;
        [SerializeField] private AudioClip _pressClip;
        [SerializeField] private AudioClip _musicClip;

        private Action<AudioClip> OnPlayMusic;
        private Action<AudioClip> OnPlaySound;
        private Action<AudioClip> OnPlayVoice;
        
        private void Start() => 
            OnPlayMusic?.Invoke(_musicClip);
        
        private void OnEnter(IMenuButtonUi menuButton) => 
            OnPlaySound?.Invoke(_hoverClip);
        
        private void OnPress(IMenuButtonUi menuButton) => 
            OnPlaySound?.Invoke(_pressClip);

        private void OnInitialize(IManager manager)
        {
            if (manager is not AudioManager audioManager)
                return;

            OnPlayMusic += audioManager.PlayMusic;
            OnPlaySound += audioManager.PlaySound;
            OnPlayVoice += audioManager.PlayVoice;
        }

        private void OnEnable()
        {
            IMenuController<IMenuButtonUi> _controller = GetComponent<IMenuController<IMenuButtonUi>>();
            _controller.OnEnter += OnEnter;
            _controller.OnPress += OnPress;
            
            GameManager.Instance.OnInitialize += OnInitialize;
        }
        
        private void OnDisable()
        {
            IMenuController<IMenuButtonUi> _controller = GetComponent<IMenuController<IMenuButtonUi>>();
            _controller.OnEnter -= OnEnter;
            _controller.OnPress -= OnPress;
            
            GameManager.Instance.OnInitialize -= OnInitialize;
        }
    }
}