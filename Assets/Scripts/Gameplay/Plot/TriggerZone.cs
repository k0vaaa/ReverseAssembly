using System.Collections;
using Core.Events;
using Core.Input;
using DG.Tweening;
using Gameplay.Events;
using Plugins.Demigiant.DOTween.Modules;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
// Обязательно для корутин

namespace Gameplay.Plot
{
    public class TriggerZone : MonoBehaviour
    {
        [SerializeField] private Volume _postProcessVolume;
        private ColorAdjustments _colorAdjustments;
        [SerializeField] private CanvasGroup _hud;

        [SerializeField] private CanvasGroup _hintGroup;
        [SerializeField] private TextMeshProUGUI _hintText;
        [SerializeField] private string _hintMessage;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private float _displayDuration = 3f; // Сколько секунд висит подсказка

        [Header("Final Trigger Settings")] [SerializeField]
        private bool _isFinalTrigger = false;

        [SerializeField] private float _finalFadeDuration = 3.0f;

        private Coroutine _fadeCoroutine;
        [Inject] private InputManager _input;
        private void Start()
        {
            if (_postProcessVolume.profile.TryGet(out _colorAdjustments))
            {
                _colorAdjustments.postExposure.value = 0f;
            }
        }

        private void Awake()
        {
            if (_hintGroup != null) _hintGroup.alpha = 0f;
            EventBus.Subscribe<BranchSwitchedEvent>(HandleBranchSwitch);
        }

        private void HandleBranchSwitch(BranchSwitchedEvent e)
        {
            if (e.NewBranch == WorldBranch.Alpha)
            {
                _hintGroup?.DOFade(0f, _fadeDuration);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (_hintText != null) _hintText.text = _hintMessage;

                if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

                _fadeCoroutine = StartCoroutine(HandleTrigger());
            }
        }

        

        private IEnumerator HandleTrigger()
        {
            if (_isFinalTrigger)
            {
                _hud?.DOFade(0f, 2);
                _input.DisablePlayerInput();
                yield return new WaitForSeconds(_fadeDuration); // Ждем пока подсказка скроется
                DOTween.To(() => _colorAdjustments.postExposure.value,
                    x => _colorAdjustments.postExposure.value = x,
                    -10f,
                    _finalFadeDuration).OnComplete(() => EventBus.Raise(new GameEndedEvent()));

            }

            _hintGroup?.DOFade(1f, _fadeDuration);

            yield return new WaitForSeconds(_displayDuration);

            _hintGroup?.DOFade(0f, _fadeDuration);
        }
    }
}
    