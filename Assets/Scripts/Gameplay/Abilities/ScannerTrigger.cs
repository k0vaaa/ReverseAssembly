using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Gameplay.Abilities
{
    public class ScannerTrigger : MonoBehaviour
    {
        [SerializeField] private SphereCollider _collider;
        [SerializeField] private float _endSize = 100f;
        [SerializeField] private float _time = 2.5f;
        private TweenerCore<float, float, FloatOptions> _tween;
        public event Action<Collider> OnTriggerEntered; 
        
        public void Expand()
        {
            _collider.enabled = true;
            _tween = DOTween.To(() => _collider.radius, value => _collider.radius = value, _endSize, _time)
                .OnComplete(() =>
                {
                    _collider.radius = 0.001f;
                    _collider.enabled = false;
                });
        }

        public void StopExpand()
        {
            _tween.Kill();
            _collider.radius = 0.001f;
            _collider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEntered?.Invoke(other);
        }
    }
}