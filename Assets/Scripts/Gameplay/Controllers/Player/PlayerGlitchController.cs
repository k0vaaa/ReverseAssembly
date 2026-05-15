using System;
using Core.Bootstrap;
using Core.DI;
using Core.Events;
using Core.Extensions;
using Gameplay.Events;
using UnityEngine;

namespace Gameplay.Controllers.Player
{
    public class PlayerGlitchController : MonoBehaviour, IInjectable, IInitializable
    {
        [Header("Visuals")]
        [SerializeField] private SkinnedMeshRenderer _playerRenderer;
        
        // Переменные для кеширования
        private Material _playerMaterial;
        public bool IsCriticallyGlitched { get; private set; } // Читается в стейт-машине

        public void Init()
        {
            if (_playerRenderer != null)
            {
                _playerMaterial = _playerRenderer.material;
            }
            
            EventBus.Subscribe<PlayerStabilityChangedEvent>(OnStabilityChanged).AddTo(gameObject);
        }

        private void OnStabilityChanged(PlayerStabilityChangedEvent eventData)
        {
            IsCriticallyGlitched = eventData.IsGlitched;

            // Обновляем материал (Шейдер должен иметь свойство _GlitchIntensity)
            if (_playerMaterial != null)
            {
                // Если глитч критический (ХП < 30%) - ставим 1. Иначе - легкий эффект от потери ХП
                float intensity = IsCriticallyGlitched ? 1f : (1f - eventData.StabilityPercent) * 0.3f;
                _playerMaterial.SetFloat("_GlitchIntensity", intensity);
            }
        }
    }
}