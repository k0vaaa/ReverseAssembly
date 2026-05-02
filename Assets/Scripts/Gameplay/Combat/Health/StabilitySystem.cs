// using System;
// using Controllers.Entities.HealthController.Interfaces;
// using Enemy;
// using UnityEngine;
// using UnityEngine.Events;
// using Weapons;
// using Weapons.Base;
//
// namespace Controllers.Entities.HealthController
// {
//     public class HealthSystem : MonoBehaviour, IDamageable, IHealthChange, IHittable, IKillable
//     {
//         private EnemyController enemyController;
//         private BossController bossController;
//
//         public void Awake()
//         {
//             enemyController = GetComponent<EnemyController>();
//             bossController = GetComponent<BossController>();
//             
//         }
//
//         public float Health
//         {
//             get => _health;
//             set
//             {
//                 _health = value;
//                 onHealthChanged?.Invoke(_health,MaxHealth);
//             }
//         }
//
//         public float MaxHealth { get; private set; } = 100f;
//         
//         public UnityEvent<float, float> onHealthChanged { get; } = new();
//         public UnityEvent<bool> onDeath { get; } = new();
//         public UnityEvent onHit { get; } = new();
//         [SerializeField] private float _health;
//
//         public void Init(float healthMultiplier)
//         {
//             MaxHealth *= healthMultiplier;
//             SetHealthToMax();
//         }
//
//         private void SetHealthToMax()
//         {
//             Health = MaxHealth;
//         }
//         
//         public void SetHealth(float health)
//         {
//             Health = health;
//         }
//
//         public void TakeDamage(Damage damage)
//         {   
//             if (Health - damage.Value <= 0)
//             {   
//                 
//                 if(enemyController){enemyController.isDead = true;}
//                 else if(bossController){bossController.isDead = true;}
//                 
//                 Health = 0;
//                 onDeath?.Invoke(true);
//             }
//             else
//             {
//                 Health -= damage.Value;
//                 onHit?.Invoke();
//             }
//         }
//  
//     }
//
//     public interface IKillable
//     {
//         public UnityEvent<bool> onDeath { get; }
//     }
// }
//
//

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
                        IsGlitched = false,
                        StabilityPercent = _stability / MaxStability
                    });
                }
            }
        }

        public float MaxStability { get; private set; } = 100f;

        public UnityEvent<float, float> onStabilityChanged { get; } = new();

        public UnityEvent<bool> OnDeath { get; } = new();

        public UnityEvent onHit { get; } = new();

        [SerializeField] private float _stability;

        public void Awake()
        {
            _controller = GetComponent<ICharacterController>();
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

        public void TakeDamage(Damage damage)
        {
            if (Stability - damage.Value <= 0)
            {
                if (_controller != null)
                {
                    _controller.isDead = true;
                }

                Stability = 0;
                OnDeath?.Invoke(true);
            }
            else
            {
                Stability -= damage.Value;
                onHit?.Invoke();
            }
        }

        public void Die()
        {
            
        }
    }

}