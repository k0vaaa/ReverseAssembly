using System;
using Core.Audio;

using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Views
{
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Slider _uiSlider;
        [Inject] private AudioManager _audioManager;
        
        [SerializeField] private AudioClip _uiClickClip;

        private void Awake()
        {
            _musicSlider.onValueChanged.AddListener(_audioManager.Settings.SetMusicVolume);
            _sfxSlider.onValueChanged.AddListener(_audioManager.Settings.SetSFXVolume);
            _uiSlider.onValueChanged.AddListener(_audioManager.Settings.SetUIVolume);

            _musicSlider.onValueChanged.AddListener(value => _audioManager.PlayUI(_uiClickClip, 1));
            _sfxSlider.onValueChanged.AddListener(value => _audioManager.PlayUI(_uiClickClip, 1));
            _uiSlider.onValueChanged.AddListener(value => _audioManager.PlayUI(_uiClickClip, 1));
            
            _musicSlider.value = _audioManager.Settings.GetMusicVolume();
            _sfxSlider.value = _audioManager.Settings.GetSFXVolume();
            _uiSlider.value = _audioManager.Settings.GetUIVolume();
        }
    }
}