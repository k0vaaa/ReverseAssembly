using Core.Bootstrap;
using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Combat.Health
{
    public class StabilitySystem : MonoBehaviour, IDamageable, IKillable, IInitializable
    {
        public float Stability
        {
            get => _stability;
            private set
            {
                _stability = value;
                onStabilityChanged?.Invoke(_stability, MaxStability);
            }
        }


        [field:SerializeField] public float MaxStability { get; private set; } = 100f;

        [ContextMenuItem("meow", nameof(Die))]
        [SerializeField] private float _stability;
        public UnityEvent<float, float> onStabilityChanged { get; } = new();
        [field:SerializeField] public bool IsInvincible { get; set; }

        public UnityEvent OnDeath { get; } = new();
        public UnityEvent onHit { get; } = new();

        private ICharacterController _controller;

        public void Init()
        {
            TryGetComponent<ICharacterController>(out var controller);
            _controller = controller;
            MaxStability *= 1;
            SetStabilityToMax();
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
            if (IsInvincible) return;
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
            OnDeath?.Invoke();
        }
    }

}