using Core.Bootstrap;
using Core.DI;
using Core.Events;
using DG.Tweening;
using Gameplay.Events;
using UnityEngine;

namespace Gameplay.Core
{
    public class BranchManager : MonoBehaviour, IInitializable
    {
        public WorldBranch CurrentBranch { get; private set; }

        [Header("Environments")]
        [SerializeField] private GameObject _mainBranchEnv;
        [SerializeField] private GameObject _alphaBranchEnv;

        [Header("Effects")]
        [SerializeField] private CanvasGroup _flashCanvas;
        [SerializeField] private AudioClip _jumpSound;
        private AudioSource _audioSource;

        public void Init()
        {
            if (_jumpSound != null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.playOnAwake = false;
            }

            // По умолчанию начинаем в Main
            SwitchBranch(WorldBranch.Main, true);
        }

        public void SwitchBranch(WorldBranch branch, bool instant = false)
        {
            CurrentBranch = branch;

            // Жестко переключаем объекты окружения
            if (_mainBranchEnv != null) _mainBranchEnv.SetActive(CurrentBranch == WorldBranch.Main);
            if (_alphaBranchEnv != null) _alphaBranchEnv.SetActive(CurrentBranch == WorldBranch.Alpha);

            if (!instant)
            {
                // Визуальный эффект вспышки
                if (_flashCanvas != null)
                {
                    // Делаем вспышку непрозрачной, затем плавно гасим
                    _flashCanvas.alpha = 1f;
                    _flashCanvas.DOFade(0f, 0.5f).SetEase(Ease.OutQuad);
                }

                // Эффект тряски камеры
                if (Camera.main != null)
                {
                    Camera.main.DOComplete(); // Останавливаем предыдущую тряску, если есть
                    Camera.main.DOShakePosition(0.4f, 0.3f, 20, 90f, false, ShakeRandomnessMode.Harmonic);
                }

                // Звук
                if (_audioSource != null && _jumpSound != null)
                {
                    _audioSource.PlayOneShot(_jumpSound);
                }
            }

            // Рассылаем глобальное событие (для звуков, партиклов или ИИ)
            EventBus.Raise(new BranchSwitchedEvent(CurrentBranch));
        }

        // Метод для вызова из Терминала
        public void ToggleBranch()
        {
            SwitchBranch(CurrentBranch == WorldBranch.Main ? WorldBranch.Alpha : WorldBranch.Main);
        }
    }
}