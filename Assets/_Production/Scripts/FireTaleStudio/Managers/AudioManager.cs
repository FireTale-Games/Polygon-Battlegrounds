using System;
using FTS.Tools.ExtensionMethods;
using FTS.Tools.ScriptableEvents;
using FTS.UI.Settings;
using UnityEngine;

namespace FTS.Managers
{
    [RequireComponent(typeof(AudioSource)), DisallowMultipleComponent]
    internal sealed class AudioManager : MonoBehaviour
    {
        private EventInvoker<ISetting> OnSettingData => _onSettingData ??= ExtensionMethods.LoadEventObject<ISetting>(nameof(OnSettingData));
        private EventInvoker<ISetting> _onSettingData;
        
        private EventInvoker<AudioClip> OnPlaySound => _onPlaySound ??= ExtensionMethods.LoadEventObject<AudioClip>(nameof(OnPlaySound));
        private EventInvoker<AudioClip> _onPlaySound;
        
        private EventInvoker<AudioClip> OnPlayMusic => _onPlayMusic ??= ExtensionMethods.LoadEventObject<AudioClip>(nameof(OnPlayMusic));
        private EventInvoker<AudioClip> _onPlayMusic;
        
        private EventInvoker<AudioClip> OnPlayVoice => _onPlayVoice ??= ExtensionMethods.LoadEventObject<AudioClip>(nameof(OnPlayVoice));
        private EventInvoker<AudioClip> _onPlayVoice;
        
        private AudioSource MusicSource => _musicSource ??= GetComponents<AudioSource>()[0];
        private AudioSource _musicSource;
        
        private AudioSource SoundSource => _soundSource ??= GetComponents<AudioSource>()[1];
        private AudioSource _soundSource;
        
        private AudioSource VoiceSource => _voiceSource ??= GetComponents<AudioSource>()[2];
        private AudioSource _voiceSource;

        [SerializeField] private float masterVolume;
        [SerializeField] private float musicVolume;
        [SerializeField] private float soundVolume;
        [SerializeField] private float voiceVolume;
        
        private void SettingData(ISetting setting)
        {
            if (setting is MasterSettingUi)
            {
                masterVolume = Convert.ToByte(setting.Value) / 100.0f;
                MusicSource.volume = masterVolume * musicVolume;
                SoundSource.volume = masterVolume * soundVolume;
                VoiceSource.volume = masterVolume * voiceVolume;
            }
            else if (setting is MusicSettingUi)
            {
                musicVolume = Convert.ToByte(setting.Value) / 100.0f;
                MusicSource.volume = masterVolume * musicVolume;
            }
            else if (setting is SoundSettingUi)
            {
                soundVolume = Convert.ToByte(setting.Value) / 100.0f;
                SoundSource.volume = masterVolume * soundVolume;
            }
            else if (setting is VoiceSettingUi)
            {
                voiceVolume = Convert.ToByte(setting.Value) / 100.0f;
                VoiceSource.volume = masterVolume * voiceVolume;
            }
        }
        
        private void PlayMusic(AudioClip audioClip)
        {
            MusicSource.clip = audioClip;
            MusicSource.Play();
        }
        
        private void PlaySound(AudioClip audioClip) => 
            SoundSource.PlayOneShot(audioClip);
        
        private void PlayVoice(AudioClip audioClip)
        {
            VoiceSource.clip = audioClip;
            VoiceSource.Play();
        }
        
        private void Awake()
        {
            OnSettingData.Null()?.AddObserver(SettingData);
            OnPlaySound.Null()?.AddObserver(PlaySound);
            OnPlayMusic.Null()?.AddObserver(PlayMusic);
            OnPlayVoice.Null()?.AddObserver(PlayVoice);
        }

        private void OnDestroy()
        {
            OnSettingData.Null()?.RemoveObserver(SettingData);
            OnPlaySound.Null()?.RemoveObserver(PlaySound);
            OnPlayMusic.Null()?.RemoveObserver(PlayMusic);
            OnPlayVoice.Null()?.RemoveObserver(PlayVoice);
        }
    }
}