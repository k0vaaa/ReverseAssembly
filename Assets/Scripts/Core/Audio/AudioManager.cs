using System;
using System.Threading.Tasks;

using Core.Events;
using Core.Pause;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace Core.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public VolumeSettings Settings { get; private set; }
        
        [SerializeField] private AudioSource _musicSourceA;
        [SerializeField] private AudioSource _musicSourceB;
        [SerializeField] private AudioSource _uiSource;
        [SerializeField] private bool _sfxIgnorePause;
        [SerializeField] private AudioMixer _mainMixer;
        private ObjectPool<AudioSource> _sfxSources;
        private GameObject _sfxContainer;


        private void Awake()
        {
            Settings = new VolumeSettings(_mainMixer);
            _sfxContainer = new GameObject("SfxContainer");
            CreateSfxAudioSource();

            ConfigureMusicSources();

            _uiSource.outputAudioMixerGroup = Settings.UIGroup;
        }

        private void ConfigureMusicSources()
        {
            _musicSourceA.outputAudioMixerGroup = Settings.MusicGroup;
            _musicSourceB.outputAudioMixerGroup = Settings.MusicGroup;

            _musicSourceA.loop = true;
            _musicSourceB.loop = true;

            _musicSourceA.ignoreListenerPause = true;
            _musicSourceB.ignoreListenerPause = true;
        }

        private void CreateSfxAudioSource()
        {
            _sfxSources = new ObjectPool<AudioSource>(
                () =>
                {
                    var go = new GameObject($"sfxSource{_sfxSources.CountAll}");
                    go.transform.SetParent(_sfxContainer.transform);
                    go.transform.position = Vector3.zero;
                    go.SetActive(false);
                    var source = go.AddComponent<AudioSource>();
                    source.outputAudioMixerGroup = Settings.SFXGroup;
                    return source;
                },
                source => source.gameObject.SetActive(true),
                source => source.gameObject.SetActive(false)
            );
        }

        private void Start()
        {
            EventBus.Subscribe<GamePauseEvent>(OnGamePaused);
        }

        private void Play(AudioClip clip, Vector3 pos, float spatialBlend, float volume, bool ignorePause, AudioSource source)
        {
            source.transform.position = pos;
            source.spatialBlend = spatialBlend;
            source.volume = volume;
            source.ignoreListenerPause = ignorePause;

            source.PlayOneShot(clip);
            
        }

        public async Task PlaySFX(AudioClip clip, Vector3 pos, float volume)
        {
            var source = _sfxSources.Get();
            
            bool ignorePause;
            if (_sfxIgnorePause)
            {
                ignorePause = false;
            }
            else
            {
                ignorePause = true;
            }

            Play(clip, pos, 1, volume, ignorePause, source);
            
            try
            {
                if (ignorePause)
                {
                    await Task.Delay((int)(clip.length * 1000), destroyCancellationToken);
                }
                else
                {
                    await Awaitable.WaitForSecondsAsync(clip.length, destroyCancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                if (this != null && source != null)
                {
                    _sfxSources.Release(source);
                }
            }
        }

        public void PlayUI(AudioClip clip, float volume)
        {
            Play(clip, Vector3.zero, 0, volume, true, _uiSource);
        }

        private void OnGamePaused(GamePauseEvent eventData)
        {
            Settings.SetPause(eventData.IsPaused);
            if (eventData.IsPaused)
            {
                Settings.SetSFXLowPass(600);
                Settings.SetMusicVolume(.2f);
            }
            else
            {
                Settings.SetSFXLowPass(22000);
            }
        }

        private void OnDestroy()
        {
            _sfxSources.Dispose();
        }
    }
}