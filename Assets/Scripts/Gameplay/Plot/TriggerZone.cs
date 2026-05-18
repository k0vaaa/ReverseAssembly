using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.Rendering; // Обязательно для корутин
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] private Volume _postProcessVolume;
    private ColorAdjustments _colorAdjustments;
    [SerializeField] private CanvasGroup HUD;

    [SerializeField] private CanvasGroup _hintGroup;
    [SerializeField] private TextMeshProUGUI hinttext;
    [SerializeField] private string _hintMessage;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private float _displayDuration = 3f; // Сколько секунд висит подсказка

    [Header("Final Trigger Settings")] [SerializeField]
    private bool _isFinalTrigger = false;

    [SerializeField] private float _finalFadeDuration = 3.0f;

    private Coroutine _fadeCoroutine;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hinttext != null) hinttext.text = _hintMessage;

            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

            _fadeCoroutine = StartCoroutine(HandleTrigger());
        }
    }

    private IEnumerator HandleTrigger()
    {
        _hintGroup?.DOFade(1f, _fadeDuration);

        yield return new WaitForSeconds(3);

        _hintGroup?.DOFade(0f, _fadeDuration);
        

        if (_isFinalTrigger)
        {
            HUD?.DOFade(0f, 2);
            yield return new WaitForSeconds(_fadeDuration); // Ждем пока подсказка скроется
            DOTween.To(() => _colorAdjustments.postExposure.value,
                x => _colorAdjustments.postExposure.value = x,
                -10f,
                _finalFadeDuration);

        }
    }
}
    