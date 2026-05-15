using Core.Bootstrap;
using Core.DI;
using Core.Events;
using Core.Extensions;
using Gameplay.Events;
using UnityEngine;
using UnityEngine.Rendering;

namespace Gameplay.Controllers.Player
{
    public class ScreenGlitchController : MonoBehaviour, IInjectable, IInitializable
    {
        [Inject] private Volume _postProcessVolume;
        
        [Header("Audio")]
        [SerializeField] private AudioClip _glitchLoopClip;
        
        private AudioSource _audioSource;

        public void Init()
        {
            if (_glitchLoopClip != null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.clip = _glitchLoopClip;
                _audioSource.loop = true;
                _audioSource.playOnAwake = false;
                _audioSource.volume = 0.5f;
            }

            EventBus.Subscribe<PlayerStabilityChangedEvent>(OnStabilityChanged).AddTo(gameObject);
        }

        private void OnStabilityChanged(PlayerStabilityChangedEvent eventData)
        {
            if (_postProcessVolume != null && _postProcessVolume.profile != null)
            {
                // In a real implementation, you would try to get ChromaticAberration and LensDistortion
                // and adjust their intensities. Since we are avoiding hard dependencies on URP/HDRP packages here,
                // we leave this comment to indicate where those values are modified.
                
                // Example for URP:
                if (_postProcessVolume.profile.TryGet<UnityEngine.Rendering.Universal.ChromaticAberration>(out var chromaticAberration))
                {
                    chromaticAberration.intensity.value = eventData.IsGlitched ? 1f : 0f;
                }
                if (_postProcessVolume.profile.TryGet<UnityEngine.Rendering.Universal.LensDistortion>(out var lensDistortion))
                {
                    lensDistortion.intensity.value = eventData.IsGlitched ? -0.5f : 0f;
                }
            }

            if (eventData.IsGlitched)
            {
                if (_audioSource != null && !_audioSource.isPlaying)
                {
                    _audioSource.Play();
                }
            }
            else
            {
                if (_audioSource != null && _audioSource.isPlaying)
                {
                    _audioSource.Stop();
                }
            }
        }
    }
}
