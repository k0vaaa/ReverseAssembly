using Core.Events;
using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using Gameplay.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Combat.Health
{
    public class StabilitySystem : MonoBehaviour, IDamageable, IHealthChange, IHittable, IKillable
    {
        private ICharacterController _controller;

        public float Stability
        {
            get => _stability;
            private set
            {
                _stability = value;
                onStabilityChanged?.Invoke(_stability, MaxStability);
                if (_controller == null)
                {
                    EventBus.Raise(new PlayerStabilityChangedEvent()
                    {
                        IsGlitched = (_stability / MaxStability) < 0.3f,
                        StabilityPercent = _stability / MaxStability
                    });
                }
            }
        }

        [field:SerializeField] public float MaxStability { get; private set; } = 100f;
        
        public UnityEvent<float, float> onStabilityChanged { get; } = new();

        public UnityEvent<bool> OnDeath { get; } = new();

        public UnityEvent onHit { get; } = new();

        [ContextMenuItem("meow", nameof(Die))]
        [SerializeField] private float _stability;

        public void Awake()
        {
            TryGetComponent<ICharacterController>(out var controller);
            _controller = controller;
        }

        public void Init(float stabilityMultiplier)
        {
            MaxStability *= stabilityMultiplier;
            SetStabilityToMax();
        }

        private void SetStabilityToMax()
        {
            Stability = MaxStability;
        }

        public void SetStability(float stability)
        {
            Stability = stability;
        }

        [ContextMenu("TakeDamage10")]
        public void TakeDamageTest() => TakeDamage(new Damage(DamageType.Physic,10));
        
        public void TakeDamage(Damage damage)
        {
            if (Stability - damage.Value <= 0)
            {
                if (_controller != null)
                {
                    _controller.isDead = true;
                }

                Stability = 0;
                Die();
            }
            else
            {
                Stability -= damage.Value;
                onHit?.Invoke();
            }
        }

        public void Die()
        {
            OnDeath?.Invoke(true);
        }
    }

}