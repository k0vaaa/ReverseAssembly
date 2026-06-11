using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class GlitchVFX : MonoBehaviour
    {
        private SkinnedMeshRenderer _renderer;
        private MaterialPropertyBlock _propBlock;

        [Header("Настройки")] [SerializeField] private float _glitchDuration = 0.2f;

        private Tween _glitchTween;

        private readonly int _glitchIntensityProp = Shader.PropertyToID("_GlitchIntensity");

        private void Start()
        {
            _renderer = GetComponent<SkinnedMeshRenderer>();
            _propBlock = new MaterialPropertyBlock(); // Создаем блок для передачи данных

            _glitchTween = DOVirtual.Float(1f, 0f, _glitchDuration, intensity =>
                {
                    // Этот код будет вызываться DOTween'ом каждый кадр анимации
                    _renderer.GetPropertyBlock(_propBlock);
                    _propBlock.SetFloat(_glitchIntensityProp, intensity);

                    // Применяем ко ВСЕМ материалам на этом рендерере разом!
                    _renderer.SetPropertyBlock(_propBlock);
                })
                .SetAutoKill(false)
                .Pause();
        }

        public void DoGlitch()
        {
            _glitchTween?.Restart();
        }

        private void OnDestroy()
        {
            _glitchTween?.Kill();
        }
    }
}