using DG.Tweening;
using UnityEngine;

namespace Gameplay.VFX
{
    public class BridgeMaterializer : MonoBehaviour
    {
        [Header("Shader Settings")]
        [SerializeField]
        private string _propertyName = "_Value";

        [Header("Animation Settings")]
        [SerializeField]
        private float _delay = 3.0f;

        [SerializeField] private float _duration = 2.0f;
        [SerializeField] private Ease _ease = Ease.OutQuad;

        private Renderer[] _renderers;
        private MaterialPropertyBlock _propBlock;

        private void Awake()
        {
            _renderers = GetComponentsInChildren<Renderer>();
            _propBlock = new MaterialPropertyBlock();
        }

        private void SetDissolve(float val)
        {
            if (_renderers == null) return;

            foreach (var r in _renderers)
            {
                if (r == null) continue;

                r.GetPropertyBlock(_propBlock);
                _propBlock.SetFloat(_propertyName, val);
                r.SetPropertyBlock(_propBlock);
            }
        }

        public void SetBridge(bool active, float duration, float delay = 0, Ease ease = Ease.InOutQuad)
        {
            float currentValue = active ? 1f : 0f;
            float endValue = active ? 0f : 1f;
            DOTween.To(() => currentValue, x =>
                {
                    currentValue = x;
                    SetDissolve(currentValue);
                }, endValue, duration)
                .SetDelay(delay)
                .SetEase(ease)
                .OnKill(() => GetComponentInChildren<Collider>().enabled = active);
        }
    }
}