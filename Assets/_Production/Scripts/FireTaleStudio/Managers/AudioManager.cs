using System;
using FTS.UI.Settings;
using UnityEngine;

namespace FTS.Managers
{
    [RequireComponent(typeof(AudioSource)), DisallowMultipleComponent]
    internal sealed class AudioManager : BaseManager
    {
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

        private void OnEnable() => 
            GameManager.Instance.OnInitialize += OnInitialize;

        private void OnInitialize(IManager manager)
        {
            if (manager is SettingManager settingManager)
                BindToSettingsManager(settingManager);
        }
        
        private void BindToSettingsManager(SettingManager settingManager) => 
            settingManager.OnSettingApplied += SettingData;

        private void SettingData(object sender, ISetting setting)
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
        
        public void PlayMusic(AudioClip audioClip)
        {
            MusicSource.clip = audioClip;
            MusicSource.Play();
        }
        
        public void PlaySound(AudioClip audioClip) => 
            SoundSource.PlayOneShot(audioClip);
        
        public void PlayVoice(AudioClip audioClip)
        {
            VoiceSource.clip = audioClip;
            VoiceSource.Play();
        }
    }
}