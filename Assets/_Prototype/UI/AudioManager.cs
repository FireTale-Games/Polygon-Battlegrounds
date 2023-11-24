using FTS.Tools.ScriptableEvents;
using UnityEngine;

namespace FTS.Managers
{
    [RequireComponent(typeof(AudioSource)), DisallowMultipleComponent]
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private EventObserver<AudioClip> _playSound;
        [SerializeField] private EventObserver<AudioClip> _playMusic;

        private AudioSource AudioSource => _audioSource ??= GetComponent<AudioSource>();
        private AudioSource _audioSource;
        
        private void OnEnable()
        {
            _playSound.AddObserver(OnPlaySound);
            _playMusic.AddObserver(OnPlayMusic);
        }

        private void OnDisable()
        {
            _playSound.RemoveObserver(OnPlaySound);
            _playMusic.RemoveObserver(OnPlayMusic);
        }

        private void OnPlaySound(AudioClip audioClip) => 
            AudioSource.PlayOneShot(audioClip);

        private void OnPlayMusic(AudioClip audioClip)
        {
            Debug.Log("Hello");
            AudioSource.clip = audioClip;
            AudioSource.Play();
        }
    }
}