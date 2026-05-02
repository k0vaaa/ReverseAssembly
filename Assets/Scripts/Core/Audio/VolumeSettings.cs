using Core.Utilities;
using UnityEngine;
using UnityEngine.Audio;

namespace Core.Audio
{
    public class VolumeSettings
    {
        public AudioMixer MainMixer => _mainMixer;
        public AudioMixerGroup MusicGroup => _musicGroup;
        public AudioMixerGroup SFXGroup => _sfxGroup;
        public AudioMixerGroup UIGroup => _uiGroup;

        private readonly AudioMixer _mainMixer;
        private readonly AudioMixerGroup _musicGroup;
        private readonly AudioMixerGroup _sfxGroup;
        private readonly AudioMixerGroup _uiGroup;

        private const string MasterParam = "MasterVolume";
        private const string MusicParam = "MusicVolume";
        private const string SFXParam = "SFXVolume";
        private const string UIParam = "UIVolume";
        private const string SFXLowPassParam = "SFXLowPass";


        public VolumeSettings(AudioMixer mainMixer)
        {
            _mainMixer = mainMixer;
            _musicGroup = mainMixer.FindMatchingGroups("Music")[0];
            _sfxGroup = mainMixer.FindMatchingGroups("SFX")[0];
            _uiGroup = mainMixer.FindMatchingGroups("UI")[0];
        }

        public void RetrieveVolumeSettings()
        {
            
        }

        public void SetMasterVolume(float value)
        {
            value = Utils.ToDBf(value);
            _mainMixer.SetFloat(MasterParam, value);
        }

        public float GetMasterVolume()
        {
            _mainMixer.GetFloat(MasterParam, out float value);
            return Utils.FromDBf(value);
        }

        public void SetMusicVolume(float value)
        {
            value = Utils.ToDBf(value);
            _mainMixer.SetFloat(MusicParam, value);
        }

        public float GetMusicVolume()
        {
            _mainMixer.GetFloat(MusicParam, out float value);
            return Utils.FromDBf(value);
        }

        public void SetSFXVolume(float value)
        {
            value = Utils.ToDBf(value);
            _mainMixer.SetFloat(SFXParam, value);
        }

        public float GetSFXVolume()
        {
            _mainMixer.GetFloat(SFXParam, out float value);
            return Utils.FromDBf(value);
        }

        public void SetUIVolume(float value)
        {
            value = Utils.ToDBf(value);
            _mainMixer.SetFloat(UIParam, value);
        }

        public float GetUIVolume()
        {
            _mainMixer.GetFloat(UIParam, out float value);
            return Utils.FromDBf(value);
        }

        public void SetSFXLowPass(float value)
        {
            _mainMixer.SetFloat(SFXLowPassParam, value);
        }

        public float GetSFXLowPass()
        {
            _mainMixer.GetFloat(SFXLowPassParam, out float value);
            return value;
        }


        public void SetMusicOutputGroup(AudioSource source)
        {
            source.outputAudioMixerGroup = _musicGroup;
        }

        public void SetSFXOutputGroup(AudioSource source)
        {
            source.outputAudioMixerGroup = _sfxGroup;
        }

        public void SetPause(bool isPaused)
        {
            AudioListener.pause = isPaused;
        }
    }
}