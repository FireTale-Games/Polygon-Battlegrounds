using FTS.Tools.ExtensionMethods;
using FTS.Tools.ScriptableEvents;
using UnityEngine;

namespace FTS.Managers
{
    [RequireComponent(typeof(AudioSource)), DisallowMultipleComponent]
    public class AudioManager : MonoBehaviour
    {
        private EventInvoker<AudioClip> OnPlaySound => _onPlaySound ??= ExtensionMethods.LoadEventInvoker<AudioClip>(nameof(OnPlaySound));
        private EventInvoker<AudioClip> _onPlaySound;
        
        private EventInvoker<AudioClip> OnPlayMusic => _onPlayMusic ??= ExtensionMethods.LoadEventInvoker<AudioClip>(nameof(OnPlayMusic));
        private EventInvoker<AudioClip> _onPlayMusic;
        
        private AudioSource AudioSource => _audioSource ??= GetComponent<AudioSource>();
        private AudioSource _audioSource;

        private void PlaySound(AudioClip audioClip) => 
            AudioSource.PlayOneShot(audioClip);

        private void PlayMusic(AudioClip audioClip)
        {
            AudioSource.clip = audioClip;
            AudioSource.Play();
        }
        
        private void OnEnable()
        {
            OnPlaySound.AddObserver(PlaySound);
            OnPlayMusic.AddObserver(PlayMusic);
        }

        private void OnDisable()
        {
            OnPlaySound.RemoveObserver(PlaySound);
            OnPlayMusic.RemoveObserver(PlayMusic);
        }
    }
}